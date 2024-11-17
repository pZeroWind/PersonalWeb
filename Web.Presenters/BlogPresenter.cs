using Markdig;
using System.Text.RegularExpressions;
using Web.Managers;
using Web.Managers.DbTables;
using Web.Models;

namespace Web.Presenters
{
    public class BlogPresenter : BasePresenter<BlogSearch>
    {
        private readonly SqliteManager _sqlite;

        public BlogPresenter(SqliteManager sqlite)
        {
            _sqlite = sqlite;
        }

        /// <summary>
        /// 博客列表
        /// </summary>
        public IEnumerable<Blog> GetBlogs(string? title)
        {
            Model.Title = title ?? string.Empty;
            return _sqlite.Client.Queryable<BlogTable>()
                .Where(item => item.FileName.Contains(Model.Title))
                .ToPageList(Model.Page, Model.Size)
                .Select(item => new Blog(item));
        }

        public Blog GetBlog(string id)
        {
            var table = _sqlite.Client.Queryable<BlogTable>()
                .First(blog => blog.Id == id.ToID());

            var blog = new Blog(table);

            if (blog.Content != null)
            {
                var htmlContent = Markdown.ToHtml(blog.Content);

                string pattern = @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>";
                // 替换 src 属性的值
                blog.Content = Regex.Replace(htmlContent, pattern, match =>
                {
                    // 获取匹配到的 src 属性的值
                    string oldSrc = match.Groups[1].Value;

                    // 仅替换相对路径（避免替换外部 URL）
                    if (!oldSrc.StartsWith("http"))
                    {
                        string newSrc = $"{blog.FileName}/{oldSrc}";
                        return match.Value.Replace(oldSrc, newSrc);  // 返回替换后的 img 标签
                    }

                    return match.Value;  // 如果是外部 URL，不做替换
                });
            }
            return blog;
        }

    
        /// <summary>
        /// 获取总数
        /// </summary
        public int GetTotal()
        {
            return _sqlite.Client.Queryable<BlogTable>().Count();
        }

        /// <summary>
        /// 获取最后一页索引
        /// </summary>
        public int LastPageIndex()
        {
            return (int)Math.Ceiling(GetTotal() / (Model.Size * 1f));
        }

        /// <summary>
        /// 下一页索引
        /// </summary>
        public int NextPageIndex()
        {
            if (GetTotal() <= Model.Size * Model.Page)
            {
                return Model.Page;
            }
            return ++Model.Page;
        }

        public int ToPageIndex(int page)
        {
            if (page >= LastPageIndex() || page <= 0)
            {
                return Model.Page;
            }
            Model.Page = page;
            return Model.Page;
        }

        /// <summary>
        /// 上一页索引
        /// </summary>
        /// <returns></returns>
        public int PrevPageIndex()
        {
            if (Model.Page == 1)
            {
                return Model.Page;
            }
            return --Model.Page;
        }

        /// <summary>
        /// 当前索引
        /// </summary>
        /// <returns></returns>
        public int GetCurrentPageIndex() => Model.Page;

        /// <summary>
        /// 页面大小
        /// </summary>
        public int GetPageSize() => Model.Size;

        /// <summary>
        /// 设置页面大小
        /// </summary>
        public void SetPageSize(int size) => Model.Size = size; 
    }
}
