using MyWebsite.Dtos.ArticleDtos;
using MyWebsite.Dtos.ProjectDtos;

public class ReadCategoryDtos
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Slug { get; set; } = "";

    // İlişkiler DTO tipi olmalı
    public ICollection<ReadArticleDtos>? Articles { get; set; }
    public ICollection<ReadProjectDtos>? Projects { get; set; }
}
