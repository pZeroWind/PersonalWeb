using Web.Managers.ComponentModel;
using Web.Presenters;
using Web.Presenters.IViews;

namespace Web.Ui.ViewModel
{
    public class BlogsVModel : IBlogView
    {
        public string Search { get; set; } = string.Empty;

        public IEnumerable<Blog> BlogList { get; set; } = [];

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public int TotalCount { get; set; }
    }
}
