using System.Linq.Expressions;
using Web.Managers.ComponentModel;
using Web.Managers.DbTables;

namespace Web.Managers
{
    public class BlogModel(SqliteManager sqlite) : AsyncBaseModel<BlogTable>(sqlite)
    {

        /// <summary>
        /// 标题
        /// </summary>
        private string _title = string.Empty;

        /// <summary>
        /// 页面索引
        /// </summary>
        private int _pageIndex;

        /// <summary>
        /// 页面大小
        /// </summary>
        private int _pageSize;

        public async Task<List<Blog>> GetBlogsAsync(string title, int page, int size)
        {
            _title = title;
            _pageIndex = page;
            _pageSize = size;
            return (await GetListAsync(blog => blog.Name.Contains(_title)))
                .Select(blog => new Blog(blog))
                .ToList();
        }

        /// <summary>
        /// 获取最后一页索引
        /// </summary>
        public async Task<int> GetLastPageIndexAsync() => (int)Math.Ceiling((await GetCountAsync()) / (_pageSize * 1f));

        public async Task<int> GetTotalAsync() => await GetCountAsync();

        public async Task<Blog> Get(long id)
        {
            return new Blog(await FindAsync(blog => blog.Id == id));
        }

        protected override async Task<List<BlogTable>> GetListAsync(Expression<Func<BlogTable, bool>> where)
        {
            return await Sqlite.Client.Queryable<BlogTable>().Where(where).ToPageListAsync(_pageIndex, _pageSize);
        }

        protected override async Task<int> GetCountAsync()
        {
            int conut = await Sqlite.Client.Queryable<BlogTable>()
                .Where(blog => blog.Name.Contains(_title))
                .CountAsync();
            return conut > 0 ? conut : 1;
        }
    }
}
