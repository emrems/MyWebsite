namespace MyWebsite.Dtos.MediaDtos
{
    public class CreateMediaDto
    {
        public int? ArticleId { get; set; }
        public int? ProjectId { get; set; }
        public List<IFormFile> Files { get; set; } = new();
    }

}
