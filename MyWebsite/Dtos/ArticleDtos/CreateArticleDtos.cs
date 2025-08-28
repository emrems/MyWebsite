using MyWebsite.Entities;

namespace MyWebsite.Dtos.ArticleDtos
{
    public class CreateArticleDtos
    {
        public string Title { get; set; } = "";      
        public string Content { get; set; } = "";     
        public string Slug { get; set; } = "";       

        public int AuthorId { get; set; }
       
        public int? CategoryId { get; set; }
        
    }
}
