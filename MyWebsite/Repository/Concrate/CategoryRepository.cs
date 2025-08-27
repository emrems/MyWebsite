using Microsoft.EntityFrameworkCore;
using MyWebsite.Entities;
using MyWebsite.Repository.config;
using MyWebsite.Repository.Interfaces;

namespace MyWebsite.Repository.Concrate
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(MyWebSiteData dbContext) : base(dbContext)
        {
        }

        public IQueryable<Category> FindAllWithRelations()
        {
            return _DbContext.Categories
                           .Include(c => c.Articles)
                           .Include(c => c.Projects)
                           .AsQueryable();
        }

    }
}
