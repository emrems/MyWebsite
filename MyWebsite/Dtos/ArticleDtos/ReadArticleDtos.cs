namespace MyWebsite.Dtos.ArticleDtos
{
    public class ReadArticleDtos
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";       
        public string Content { get; set; } = "";     
        public string Slug { get; set; } = "";      
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
