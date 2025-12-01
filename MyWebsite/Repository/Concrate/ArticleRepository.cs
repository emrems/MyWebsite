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

        public async Task<IEnumerable<Article>> GetAllArticlesAsync()
        {
            var articles = await _DbContext.Articles
                     .Include(a => a.Likes)  
                     .ToListAsync();
            return articles;
        }

        public async Task<Article> GetArticleByIdAsync(int id)
        {
            return await _DbContext.Articles
                .Include(x => x.Likes)
                .Where(a=>a.Id==id)
                .FirstOrDefaultAsync();
                
        }

        public async Task<Article?> GetArticleBySlugAsync(string slug)
        {
            var article = await   _DbContext.Set<Article>()
                .Include(x=>x.Likes)
                .Include(x=>x.Comments)
                    .ThenInclude(x=>x.User)
                .Include(x=>x.Media)
                .FirstOrDefaultAsync(a => a.Slug == slug);
            return article;
        }

        
    }
}
