using Microsoft.EntityFrameworkCore;
using MyWebsite.Entities;
using MyWebsite.Repository.config;
using MyWebsite.Repository.Interfaces;

namespace MyWebsite.Repository.Concrate
{
    public class ArticleLikeRepository : RepositoryBase<ArticleLike>, IArticleLikeRepository
    {
        public ArticleLikeRepository(MyWebSiteData dbContext) : base(dbContext)
        {
        }

        public async Task<ArticleLike> GetLikeUserIdAndArticleId(int userId, int articleId)
        {
            var result = await _DbContext.ArticleLikes.FirstOrDefaultAsync(x => x.UserId == userId && x.ArticleId == articleId);
            return result;
            
        }
    }
}
