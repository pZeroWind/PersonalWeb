using Web.Managers.ComponentModel;

namespace Web.Presenters.IViews
{
    public interface IBlogInfoView
    {
        public string Id { get; set; }

        public Blog? Data { get; set; }
    }
}
