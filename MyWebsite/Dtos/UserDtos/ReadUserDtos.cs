using MyWebsite.Dtos.ArticleDtos;
using MyWebsite.Dtos.Comment;
using MyWebsite.Entities;

namespace MyWebsite.Dtos.UserDtos
{
    public class ReadUserDtos
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; } 
      
        public string Role { get; set; } = "User"; 
       

        // Navigation
        public ICollection<ReadArticleDtos>? Articles { get; set; }
        public ICollection<ReadCommentDto>? Comments { get; set; }
    }
}
