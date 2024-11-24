using System.Linq.Expressions;
using Web.Managers.ComponentModel;
using Web.Managers.DbTables;

namespace Web.Managers
{
    public class AdminModel(SqliteManager sqlite, SnowflakeIdGenerator snowflake) : AsyncBaseModel<AdminTable>(sqlite)
    {
        public async Task<AdminTable?> GetAdminAsync()
        {
            return (await GetListAsync(p => true)).FirstOrDefault();
        }

        public async Task<int> UpdateAdminAsync(AdminTable table)
        {
            var admin = await GetAdminAsync();
            if (admin == null)
            {
                table.Id = snowflake.GenerateId();
                return await InsertAsync(table);
            }
            table.Id = admin.Id;
            return await UpdateAsync(table);
        }
    }
}
