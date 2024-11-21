using Web.Managers;
using Web.Managers.ComponentModel;
using Web.Presenters.IViews;

namespace Web.Presenters
{
    public class BlogPresenter(BlogModel model) : BasePresenter<BlogModel, IBlogView>(model)
    {

        /// <summary>
        /// 博客列表
        /// </summary>
        public async Task GetBlogs()
        {
            if (View == null) return;
            View.BlogList = await Model.GetBlogsAsync(View.Search ?? string.Empty, View.PageIndex, View.PageSize);
            View.PageCount = await Model.GetLastPageIndexAsync();
            View.TotalCount = await Model.GetTotalAsync();
        }

        

        /// <summary>
        /// 下一页索引
        /// </summary>
        public async Task NextPageIndex()
        {
            if (View == null) return;
            if (View.TotalCount <= View.PageIndex * View.PageSize)
            {
                return;
            }
            View.PageIndex++;
            await GetBlogs();
        }

        public async Task ToPageIndex(int page)
        {
            if (View == null) return;
            int last = await Model.GetLastPageIndexAsync();
            if (page >= last)
            {
                View.PageIndex = last;
            }
            else if (page < last && page > 0)
            {
                View.PageIndex = page;
            }
            else
            {
                View.PageIndex = 1;
            }
            await GetBlogs();
        }

        /// <summary>
        /// 上一页索引
        /// </summary>
        /// <returns></returns>
        public async Task PrevPageIndex()
        {
            if (View == null) return;
            if (View.PageIndex == 1)
            {
                return;
            }
            View.PageIndex--;
            await GetBlogs();
        }
    }
}
