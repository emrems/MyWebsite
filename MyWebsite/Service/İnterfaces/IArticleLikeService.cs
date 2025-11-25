using MyWebsite.Dtos.ArticleLikeDtos;
using MyWebsite.Dtos.Response;

namespace MyWebsite.Service.İnterfaces
{
    public interface IArticleLikeService
    {
        //Task<int> getArticLikeCountByArticleİd(int articleİd);
        Task<BaseResponse<object>> createArticleLike(CreateArticleLikeDto dto);
    }
}
