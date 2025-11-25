using MyWebsite.Entities;

namespace MyWebsite.Dtos.ArticleDtos
{
    public class ReadArticleDtos
    {
        public int Id { get; set; }
        public string Title { get; set; }      
        public string Content { get; set; }   
        public string Slug { get; set; }
        public bool IsLiked { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int ArticleLikeCount { get; set; }


    }
}
