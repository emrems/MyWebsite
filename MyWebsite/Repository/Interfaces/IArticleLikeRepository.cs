using MyWebsite.Entities;
using MyWebsite.Repository.Concrate;

namespace MyWebsite.Repository.Interfaces
{
    public interface IArticleLikeRepository : IRepositoryBase<ArticleLike>
    {

        Task<ArticleLike> GetLikeUserIdAndArticleId(int userId, int articleId);
    }
}
