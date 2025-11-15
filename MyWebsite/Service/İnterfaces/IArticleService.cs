using MyWebsite.Dtos.ArticleDtos;
using MyWebsite.Dtos.Response;

namespace MyWebsite.Service.İnterfaces
{
    public interface IArticleService
    {
        Task<BaseResponse<IQueryable<ReadArticleDtos>>> GetAllArticlesAsync();
        Task<BaseResponse<ReadArticleDtos>> GetArticleByIdAsync(int id);
        Task<BaseResponse<object>> CreateArticleAsync(CreateArticleDtos article);
        Task<BaseResponse<object>> UpdateArticleAsync(UpdateArticleDtos article);
        Task<BaseResponse<object>> DeleteArticleAsync(int id);
        Task<BaseResponse<ReadArticleDtos>> GetArticleBySlugAsync(string slug);
    }
}
