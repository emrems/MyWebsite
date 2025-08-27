namespace MyWebsite.Dtos.ProjectDtos
{
    public class CreateProjectDtos
    {
        public string Title { get; set; } = "";       // Proje başlığı
        public string Description { get; set; } = ""; // Açıklama
        public string Technologies { get; set; } = ""; // Kullanılan teknolojiler
        public string GithubUrl { get; set; } = "";
        public string LiveDemoUrl { get; set; } = ""; 
        public int? CategoryId { get; set; }
    }
}
