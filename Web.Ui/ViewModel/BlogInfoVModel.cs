using Web.Managers.ComponentModel;
using Web.Presenters.IViews;

namespace Web.Ui.ViewModel
{
    public class BlogInfoVModel : IBlogInfoView
    {
        public string Id { get; set; } = string.Empty;

        public Blog? Data { get; set; }
    }
}
