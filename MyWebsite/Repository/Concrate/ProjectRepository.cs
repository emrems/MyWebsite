using Microsoft.EntityFrameworkCore;
using MyWebsite.Entities;
using MyWebsite.Repository.config;
using MyWebsite.Repository.Interfaces;

namespace MyWebsite.Repository.Concrate
{
    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        
        public ProjectRepository(MyWebSiteData context) : base(context)
        {
        }

        
         
        public IQueryable<Project> FindAllProject()
        {
            var projects = FindAll().Include(p => p.Category);
            return projects;
        }

        public async Task<Project> GetByIdAsyncProject(int id) 
        {
            return await _DbContext.Projects
                                 .Include(p => p.Category)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }



    }
}
