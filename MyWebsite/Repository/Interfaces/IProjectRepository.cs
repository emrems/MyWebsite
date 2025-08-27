using MyWebsite.Entities;

namespace MyWebsite.Repository.Interfaces
{
    public interface IProjectRepository : IRepositoryBase<Project>
    {
        IQueryable<Project> FindAllProject();
        Task<Project> GetByIdAsyncProject(int id);
    }
}
