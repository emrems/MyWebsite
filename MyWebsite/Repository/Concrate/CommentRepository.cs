using Microsoft.EntityFrameworkCore;
using MyWebsite.Entities;
using MyWebsite.Repository.config;
using MyWebsite.Repository.Interfaces;

namespace MyWebsite.Repository.Concrate
{
    public class CommentRepository : RepositoryBase<Comment>,ICommentRepository
    {
        
        public CommentRepository(MyWebSiteData _dbContext) :base(_dbContext)
        {
            
        }

        public async Task<Comment> getById(int id)
        {
            var comment = await _DbContext.Comments
                .Include(x => x.Article)
                .FirstOrDefaultAsync(x => x.ArticleId == id);
            return comment;
        }
    }
}
