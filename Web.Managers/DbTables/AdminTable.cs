using SqlSugar;

namespace Web.Managers.DbTables
{
    [SugarTable(nameof(AdminTable))]
    public class AdminTable
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? Nickname { get; set; }

        [SugarColumn(IsNullable = true)]
        public long? Birthday { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? Work { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? Email { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? Password { get; set; }
    }
}
