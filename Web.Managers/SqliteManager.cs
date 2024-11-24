using SqlSugar;
using Web.Managers.DbTables;

namespace Web.Managers
{
    public class SqliteManager
    {
        private SqlSugarClient _client;

        public SqlSugarClient Client => _client;

        public SqliteManager(ConnectionConfig connectionConfig, SnowflakeIdGenerator snowflake)
        {
            _client = new SqlSugarClient(connectionConfig);
            // 创建数据库
            _client.DbMaintenance.CreateDatabase();
            _client.CodeFirst
                .SetStringDefaultLength(255)
                .InitTables(
                    typeof(BlogTable),
                    typeof(ProjectTable),
                    typeof(SkillTable),
                    typeof(AdminTable)
                );
        }
    }
}
