using Web.Managers.ComponentModel;

namespace Web.Presenters.IViews
{
    public interface IAboutView
    {
        public string Nickname { get; set; }

        public string Birthday { get; set; }

        public string Work { get; set; }

        public string Email { get; set; }

        public List<SkillData> SkillList { get; set; }

        public List<ProjectData> ProjectExperience { get; set; }
    }

    public struct SkillData
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Level { get; set; }
    }

    public struct ProjectData
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Url { get; set; }
    }
}
