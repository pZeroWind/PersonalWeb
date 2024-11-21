using SQLitePCL;
using System.Drawing.Printing;
using System.Linq.Expressions;
using Web.Managers;
using Web.Managers.DbTables;

namespace Web.Managers
{
    public abstract class AsyncBaseModel<T>(SqliteManager sqlite) where T : class, new()
    {
        protected SqliteManager Sqlite => sqlite;

        /// <summary>
        /// 添加
        /// </summary>
        protected virtual async Task<int> InsertAsync(T item)
        {
            return await Sqlite.Client.Insertable(item).ExecuteCommandAsync();
        }

        /// <summary>
        /// 查询
        /// </summary>
        protected virtual async Task<T> FindAsync(Expression<Func<T, bool>> where)
        {
            return await Sqlite.Client.Queryable<T>().FirstAsync(where);
        }

        /// <summary>
        /// 获取总数
        /// </summary>
        protected virtual async Task<int> GetCountAsync()
        {
            return await Sqlite.Client.Queryable<T>().CountAsync();
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        protected virtual async Task<List<T>> GetListAsync(Expression<Func<T, bool>> where)
        {
            return await Sqlite.Client.Queryable<T>().Where(where).ToListAsync();
        }

        /// <summary>
        /// 删除
        /// </summary>
        protected virtual async Task<int> RemoveAsync(T item)
        {
            return await Sqlite.Client.Deleteable(item).ExecuteCommandAsync();
        }

        /// <summary>
        /// 更新
        /// </summary>
        protected virtual async Task<int> UpdateAsync(T item)
        {
            return await Sqlite.Client.Updateable(item).ExecuteCommandAsync();
        }
    }
}
