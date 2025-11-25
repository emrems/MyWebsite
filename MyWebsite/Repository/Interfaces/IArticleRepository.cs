using MyWebsite.Dtos.ArticleDtos;
using MyWebsite.Dtos.Response;
using MyWebsite.Entities;

namespace MyWebsite.Repository.Interfaces
{
    public interface IArticleRepository : IRepositoryBase<Article>
    {
        Task<Article?> GetArticleBySlugAsync(string slug);
        Task<IEnumerable<Article>> GetAllArticlesAsync();
        Task<Article> GetArticleByIdAsync(int id);
    }
}
