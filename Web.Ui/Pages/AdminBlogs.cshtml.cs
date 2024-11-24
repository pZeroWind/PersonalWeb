using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Web.Managers.ComponentModel;
using Web.Presenters;
using Web.Ui.ViewModel;

namespace Web.Ui.Pages
{
    public class AdminBlogsModel(BlogPresenter blogPresenter, BlogInfoPresenter blogInfoPresenter, AdminPresenter adminPresenter) : PageModel
    {
        private readonly BlogPresenter _blogPresenter = blogPresenter;

        private readonly BlogInfoPresenter _blogInfoPresenter = blogInfoPresenter;

        private readonly AdminPresenter _adminPresenter = adminPresenter;

        public string BlogId { get; set; } = string.Empty;

        public BlogInfoVModel Info { get; set; } = new();

        public BlogsVModel VModel { get; set; } = new();

        public string Error { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync([FromQuery] string id = "")
        {
            BlogId = id;
            if (await CheckLogin())
            {
                await ResetBlogs();
                if (!string.IsNullOrEmpty(id))
                    await _blogInfoPresenter.SetView<BlogInfoPresenter>(Info).GetBlogAsync(id);
                return Page();
            }
            return Redirect("/admin");
        }

        public async Task<IActionResult> OnPostAsync([FromForm] IFormFile[] images, [FromForm] IFormFile doc, [FromForm] string id = "", [FromForm] string title = "")
        {
            if (!(await CheckLogin()))
            {
                return Redirect("/admin");
            }
            if (string.IsNullOrEmpty(title))
            {
                Error = "标题不得为空";
                await ResetBlogs();
                return Page();
            }
            if (doc.Length <= 0)
            {
                Error = "博客文档文件不得为空";
                await ResetBlogs();
                return Page();
            }
            string date = DateTime.UtcNow.ToString("yyyy_MM_dd");
            string path = $"wwwroot/blogs/{date}/{AdminPresenter.ComputeSHA256Hash(title)}";
            string imagesPath = Path.Combine(path, "images");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!Directory.Exists(imagesPath))
                Directory.CreateDirectory(imagesPath);

            //上传图片
            foreach (var image in images)
            {
                var filePath = Path.Combine(imagesPath, image.FileName);
                // 将文件保存到指定路径
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
            }

            //上传文档
            var docPath = Path.Combine(path, doc.FileName);
            // 将文件保存到指定路径
            using (var stream = new FileStream(docPath, FileMode.Create))
            {
                await doc.CopyToAsync(stream);
            }
            if (string.IsNullOrEmpty(id))
            {
                Blog blog = new Blog
                {
                    Title = title,
                    FileName = path.Replace("wwwroot/",""),
                };
                await _blogPresenter.SetView<BlogPresenter>(VModel).AddBlogAsync(blog);
            }
            else
            {
                Blog blog = new()
                {
                    Id = id,
                    Title = title,
                    FileName = path.Replace("wwwroot/", ""),
                };
                await _blogPresenter.SetView<BlogPresenter>(VModel).UpdateBlogAsync(blog);
            }
            return Redirect("/admin/blogs");
        }

        private async Task<bool> CheckLogin()
        {
            string password = Encoding.UTF8.GetString(HttpContext.Session.Get("user") ?? []);
            if (!await _adminPresenter.CheackLoginAsync(password))
            {
                HttpContext.Session.Remove("user");
                return await Task.FromResult(false);
            }
            return await Task.FromResult(true); ;
        }

        private async Task ResetBlogs()
        {
            VModel.PageIndex = 1;
            VModel.PageSize = 5;
            await _blogPresenter.SetView<BlogPresenter>(VModel).GetBlogsAsync();
        }
    }
}
