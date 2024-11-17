using SqlSugar;

namespace Web.Managers.DbTables
{
    [SugarTable(nameof(BlogTable))]
    public class BlogTable
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(IsNullable = false)]
        public string FileName { get; set; } = string.Empty;

        [SugarColumn(IsNullable = false)]
        public long CreateTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}
