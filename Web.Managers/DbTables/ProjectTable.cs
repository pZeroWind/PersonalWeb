using SqlSugar;

namespace Web.Managers.DbTables
{
    [SugarTable(nameof(ProjectTable))]
    public class ProjectTable
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? FileName { get; set; }

        [SugarColumn]
        public long CreateTime { get; set; }
    }
}
