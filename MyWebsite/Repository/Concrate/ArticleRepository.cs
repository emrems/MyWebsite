using Microsoft.EntityFrameworkCore;
using MyWebsite.Entities;
using MyWebsite.Repository.config;
using MyWebsite.Repository.Interfaces;

namespace MyWebsite.Repository.Concrate
{
    public class ArticleRepository : RepositoryBase<Article>, IArticleRepository
    {
        public ArticleRepository(MyWebSiteData context) : base(context)
        {
        }

        public async Task<Article?> GetArticleBySlugAsync(string slug)
        {
            var article = await   _DbContext.Set<Article>().FirstOrDefaultAsync(a => a.Slug == slug);
            return article;
        }
    }
}
