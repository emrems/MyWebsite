using Microsoft.AspNetCore.Mvc.Formatters;
using MyWebsite.Enum;
namespace MyWebsite.Entities
{
    public class Media
    {
        public int Id { get; set; }
        public string Url { get; set; } = "";
        public Enum.MediaType Type { get; set; } // Image, Video

        // ilişkiler
        public int? ArticleId { get; set; }
        public Article? Article { get; set; }

        public int? ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}
