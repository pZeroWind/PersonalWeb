using Web.Managers.DbTables;

namespace Web.Managers.ComponentModel
{
    public class Blog
    {
        public Blog(BlogTable table)
        {
            Id = table.Id.ToHex();
            if (table.FileName != null)
            {
                FileName = table.FileName;
                Title = table.Name;
                using StreamReader sr = new($"wwwroot/{table.FileName}/content.md");
                Content = sr.ReadToEnd();
            }
            CreateDate = DateTimeOffset.FromUnixTimeMilliseconds(table.CreateTime).LocalDateTime.ToDateString();
        }

        public Blog()
        {
            Id = string.Empty;
        }

        public string Id;

        public string? Title;

        public string? FileName;

        public string? Content;

        public string? CreateDate;
    }
}
