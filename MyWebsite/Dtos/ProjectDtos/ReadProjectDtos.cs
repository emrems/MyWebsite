namespace MyWebsite.Dtos.ProjectDtos
{
    public class ReadProjectDtos
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string GithubUrl { get; set; }
        public string LiveDemoUrl { get; set; }
        public string Technologies { get; set; } = "";
        public string? CategoryName { get; set; } // sadece Category ismini göster
    }
}
