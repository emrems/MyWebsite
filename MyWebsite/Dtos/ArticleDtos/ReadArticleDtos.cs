using MyWebsite.Dtos.Comment;
using System.Collections.Generic; 

namespace MyWebsite.Dtos.ArticleDtos
{
    public class ReadArticleDtos
    {
        public int Id { get; set; }

        
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

        public bool IsLiked { get; set; }

       
        public string? AuthorName { get; set; }

       
        public DateTime CreatedDate { get; set; }

        public int ArticleLikeCount { get; set; }

       
        public ICollection<ReadCommentDto> Comments { get; set; } = new List<ReadCommentDto>();
    }
}