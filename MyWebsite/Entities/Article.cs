using MyWebsite.Dtos.Comment;

namespace MyWebsite.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";       // Yazı başlığı
        public string Content { get; set; } = "";     // Yazı içeriği (Markdown destekli olabilir)
        public string Slug { get; set; } = "";        // URL dostu başlık
        public bool IsLiked { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // İlişkiler
        public int AuthorId { get; set; }
        public User? Author { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<ArticleLike> Likes { get; set; } = new List<ArticleLike>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }

}
