namespace MyWebsite.Dtos.CategoryDtos
{
    public class CreateCategoryDtos
    {
       // public int Id { get; set; }
        public string Name { get; set; } = "";   // Kategori adı
        public string Slug { get; set; } = "";   // URL dostu
    }
}
