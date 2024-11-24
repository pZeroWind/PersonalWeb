using System.Linq.Expressions;
using Web.Managers.ComponentModel;
using Web.Managers.DbTables;

namespace Web.Managers
{
    public class BlogModel(SqliteManager sqlite, SnowflakeIdGenerator snowflake) : AsyncBaseModel<BlogTable>(sqlite)
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

        public async Task<Blog> GetAsync(long id)
        {
            return new Blog(await FindAsync(blog => blog.Id == id));
        }

        public async Task<int> AddAsync(Blog blog)
        {
            if (string.IsNullOrEmpty(blog.FileName)) return 0;
            BlogTable item = new BlogTable()
            {
                Id = snowflake.GenerateId(),
                Name = blog.Title ?? Guid.NewGuid().ToString("N"),
                FileName = blog.FileName
            };
            return await InsertAsync(item);
        }

        public async Task<int> UpdateAsync(Blog blog)
        {
            if (string.IsNullOrEmpty(blog.FileName)) return 0;
            BlogTable item = new BlogTable()
            {
                Id = blog.Id.ToID(),
                Name = blog.Title ?? Guid.NewGuid().ToString("N"),
                FileName = blog.FileName
            };
            return await UpdateAsync(item);
        }

        protected override async Task<List<BlogTable>> GetListAsync(Expression<Func<BlogTable, bool>> where)
        {
            return await Sqlite.Client.Queryable<BlogTable>()
                .Where(where)
                .OrderByDescending(blog => blog.CreateTime)
                .ToPageListAsync(_pageIndex, _pageSize);
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
