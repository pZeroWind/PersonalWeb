using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Policy;
using System.Text;
using Web.Presenters;
using Web.Presenters.IViews;

namespace Web.Ui.Pages
{
    public class AdminProjectsModel(AdminPresenter presenter) : PageModel
    {
        public string Error { get; set; } = string.Empty;

        public List<ProjectData> Projects { get; set; } = new List<ProjectData>();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!await CheckLogin())
            {
                return Redirect("/admin");
            }
            Projects = await presenter.GetProjectDatasAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync([FromQuery]string id)
        {
            if (!await CheckLogin())
            {
                return Redirect("/admin");
            }
            await presenter.DeleteProjectAsync(new ProjectData
            {
                Id = id
            });
            return Redirect("/admin/projects");
        }

        public async Task<IActionResult> OnPostAsync(string name, string type, string url)
        {
            if (!await CheckLogin())
            {
                return Redirect("/admin");
            }
            await presenter.AddProjectAsync(new ProjectData
            {
                Name = name,
                Type = type,
                Url = url
            });
            return Redirect("/admin/projects");
        }

        private async Task<bool> CheckLogin()
        {
            string password = Encoding.UTF8.GetString(HttpContext.Session.Get("user") ?? []);
            if (!await presenter.CheackLoginAsync(password))
            {
                HttpContext.Session.Remove("user");
                return false;
            }
            return true;
        }
    }
}
