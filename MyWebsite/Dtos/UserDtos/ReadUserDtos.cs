using MyWebsite.Dtos.ArticleDtos;
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
    }
}
