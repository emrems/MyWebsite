using MyWebsite.Dtos.ArticleDtos;

namespace MyWebsite.Service.İnterfaces
{
    public interface IArticleService
    {
        Task<IQueryable<ReadArticleDtos>> GetAllArticlesAsync();
        Task<ReadArticleDtos> GetArticleByIdAsync(int id);
        Task CreateArticleAsync(CreateArticleDtos article);
        Task UpdateArticleAsync(UpdateArticleDtos article);
        Task DeleteArticleAsync(int id);
    }
}
