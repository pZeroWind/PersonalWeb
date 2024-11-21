using Web.Managers.ComponentModel;

namespace Web.Presenters.IViews
{
    public interface IBlogView
    {
        public string Search { get; set; }

        public IEnumerable<Blog> BlogList { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public int TotalCount { get; set; }
    }
}
