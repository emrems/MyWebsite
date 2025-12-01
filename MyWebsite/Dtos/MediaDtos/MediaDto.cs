using MyWebsite.Enum;

namespace MyWebsite.Dtos.MediaDtos
{
    public class MediaDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = "";
        public MediaType Type { get; set; }
    }
}
