using MyWebsite.Entities;

namespace MyWebsite.Dtos.Comment
{
    public class ReadCommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        
        //public User User { get; set; } 
        public string UserName { get; set; }
        //public int? ArticleId { get; set; }
        
        public string ArticleTitle { get; set; }
    }
}
