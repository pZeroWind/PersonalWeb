using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Web.Presenters;

namespace Web.Ui.Pages
{

    public class AdminModel(AdminPresenter presenter) : PageModel
    {
        public string Msg { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            string password = Encoding.UTF8.GetString(HttpContext.Session.Get("user") ?? []);
            if (!await presenter.CheackLoginAsync(password))
            {
                HttpContext.Session.Remove("user");
                return Page();
            }
            return await Task.FromResult(Redirect("/admin/blogs"));
        }

        public async Task<IActionResult> OnPostAsync([FromForm] string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                Msg = "√‹¬Î≤ªµ√Œ™ø’";
                return Page();
            }
            if (!await presenter.CheackLoginAsync(password))
            {
                Msg = "√‹¬Î¥ÌŒÛ£¨«Î÷ÿ ‘";
                return Page();
            }
            HttpContext.Session.Set("user", Encoding.UTF8.GetBytes(password));
            return await Task.FromResult(Redirect("/admin/blogs"));
        }
    }
}
