using Web.Presenters.IViews;

namespace Web.Ui.ViewModel
{
    public class AboutVModel : IAboutView
    {
        public string Nickname { get; set; } = string.Empty;

        public string Birthday { get; set; } = string.Empty;

        public string Work { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public List<SkillData> SkillList { get; set; } = [];

        public List<ProjectData> ProjectExperience { get; set; } = [];
    }
}
