﻿using SqlSugar;

namespace Web.Managers.DbTables
{
    [SugarTable(nameof(ProjectTable))]
    public class ProjectTable
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? Name { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? Type { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? Url { get; set; }
    }
}
