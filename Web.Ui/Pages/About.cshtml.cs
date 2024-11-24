using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Presenters;
using Web.Ui.ViewModel;

namespace Web.Ui.Pages
{
    public class AboutModel(AdminPresenter presenter) : PageModel
    {
        public AboutVModel VModel { get; private set; } = new();

        public async Task OnGetAsync()
        {
            var admin = await presenter.GetAdminAsync();
            if (admin != null)
            {
                VModel.Nickname = admin.Nickname ?? VModel.Nickname;
                VModel.Birthday = DateTimeOffset.FromUnixTimeMilliseconds(admin.Birthday ?? 0).LocalDateTime.ToString("yyyyƒÍMM‘¬dd»’");
                VModel.Work = admin.Work ?? VModel.Work;
                VModel.Email = admin.Email ?? VModel.Email;
            }
            VModel.SkillList = await presenter.GetSkillDatasAsync();
            VModel.ProjectExperience = await presenter.GetProjectDatasAsync();
        }
    }
}
