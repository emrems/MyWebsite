namespace MyWebsite.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";       // Proje başlığı
        public string Description { get; set; } = ""; // Açıklama
        public string Technologies { get; set; } = ""; // Kullanılan teknolojiler
        public string GithubUrl { get; set; } = "";
        public string LiveDemoUrl { get; set; } = "";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

    }

}
