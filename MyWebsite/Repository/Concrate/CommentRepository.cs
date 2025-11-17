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
    }
}
