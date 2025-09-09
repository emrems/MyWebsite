using MyWebsite.Entities;

namespace MyWebsite.Repository.Interfaces
{
    public interface IArticleRepository : IRepositoryBase<Article>
    {
        Task<Article?> GetArticleBySlugAsync(string slug);
    }
}
