using Web.Managers;
using Web.Managers.DbTables;

namespace Web.Models
{
    public class Blog : IModel
    {
        public Blog(BlogTable table)
        {
            Id = table.Id.ToHex();
            if (table.FileName != null)
            {
                FileName = table.FileName;
                Title = Path.GetFileNameWithoutExtension(FileName);
                using StreamReader sr = new($"wwwroot/{table.FileName}/content.md");
                Content = sr.ReadToEnd();
            }
            CreateDate = DateTimeOffset.FromUnixTimeMilliseconds(table.CreateTime).LocalDateTime.ToString("f");
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
