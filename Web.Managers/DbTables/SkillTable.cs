using SqlSugar;

namespace Web.Managers.DbTables
{
    [SugarTable(nameof(SkillTable))]
    public class SkillTable
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? Name { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? Level { get; set; }
    }
}
