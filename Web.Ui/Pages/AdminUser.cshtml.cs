using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;
using Web.Managers.DbTables;
using Web.Presenters;

namespace Web.Ui.Pages
{
    public class AdminUserModel(AdminPresenter presenter) : PageModel
    {
        public string Error { get; set; } = string.Empty;

        public AdminTable Admin { get; set; } = new();

        public string SKill { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            if (!await CheckLogin())
            {
                return Redirect("/admin");
            }
            Admin = (await presenter.GetAdminAsync()) ?? Admin;
            SKill = await presenter.GetSkillStringAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromForm] string nickname,
            [FromForm] DateTime birthday, [FromForm] string work,[FromForm] string email, [FromForm] string password,
            [FromForm] string newPassword, [FromForm] string reqPassword, [FromForm] string skill)
        {
            if (!await CheckLogin())
            {
                return Redirect("/admin");
            }
            Admin.Nickname = nickname;
            Admin.Birthday = new DateTimeOffset(birthday).ToUnixTimeMilliseconds();
            Admin.Work = work;
            Admin.Email = email;
            Admin.Password = password;
            if (!string.IsNullOrEmpty(newPassword))
            {
                if (newPassword != reqPassword)
                {
                    Error = "两次密码不一致";
                    return Page();
                }
                Admin.Password = AdminPresenter.ComputeSHA256Hash(newPassword);
            }

            if (!string.IsNullOrEmpty(skill))
            {
                var arr = skill.Split(';');
                await presenter.UpdateSkill(arr);
            }
            await presenter.UpdateAdminAsync(Admin);
            SKill = await presenter.GetSkillStringAsync();
            return Page();
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
