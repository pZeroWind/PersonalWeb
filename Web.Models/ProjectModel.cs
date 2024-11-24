using System.Linq.Expressions;
using Web.Managers.ComponentModel;
using Web.Managers.DbTables;

namespace Web.Managers
{
    public class ProjectModel(SqliteManager sqlite, SnowflakeIdGenerator snowflake) : AsyncBaseModel<ProjectTable>(sqlite)
    {
        public async Task<List<ProjectTable>> GetAllAsync()
        {
            return await GetListAsync(p => true);
        }

        public async Task<int> AddAsync(ProjectTable table)
        {
            table.Id = snowflake.GenerateId();
            return await InsertAsync(table);
        }

        public async Task<int> DeleteAsync(ProjectTable table)
        {
            return await RemoveAsync(table);
        }
    }
}
