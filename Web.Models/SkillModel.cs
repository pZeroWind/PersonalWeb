using System.Linq.Expressions;
using Web.Managers.ComponentModel;
using Web.Managers.DbTables;

namespace Web.Managers
{
    public class SkillModel(SqliteManager sqlite, SnowflakeIdGenerator snowflake) : AsyncBaseModel<SkillTable>(sqlite)
    {
        public async Task<int> AddSKillAsync(string name, string level)
        {
            SkillTable skillTable = new SkillTable
            {
                Id = snowflake.GenerateId(),
                Name = name,
                Level = level
            };

            return await InsertAsync(skillTable);
        }

        public async Task<string> GetAllSKillStringAsync()
        {
            var list = await GetListAsync(p => true);
            return string.Join(';', list.Select(s => $"{s.Name}-{s.Level}"));
        }

        public async Task<List<SkillTable>> GetAllSKill()
        {
            return await GetListAsync(p => true);
        }

        public async Task RemoveAllAsync()
        {
            var list = await GetListAsync(p => true);
            foreach (var item in list)
            {
                await RemoveAsync(item);
            }
        }
    }
}
