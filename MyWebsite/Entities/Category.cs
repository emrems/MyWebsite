namespace MyWebsite.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";   // Kategori adı
        public string Slug { get; set; } = "";   // URL dostu
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // İlişkiler
        public ICollection<Article>? Articles { get; set; }
        public ICollection<Project>? Projects { get; set; }
    }

}
