namespace MyWebsite.Dtos.ArticleDtos
{
    public class ReadArticleDtos
    {
        public string Title { get; set; } = "";       // Yazı başlığı
        public string Content { get; set; } = "";     // Yazı içeriği (Markdown destekli olabilir)
        public string Slug { get; set; } = "";        // URL dostu başlık
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
