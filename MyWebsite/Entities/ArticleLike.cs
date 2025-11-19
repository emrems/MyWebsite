namespace MyWebsite.Entities
{
    public class ArticleLike
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
