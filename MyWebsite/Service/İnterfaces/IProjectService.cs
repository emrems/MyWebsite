using MyWebsite.Dtos.ProjectDtos;

namespace MyWebsite.Service.İnterfaces
{
    public interface IProjectService
    {
        Task<IQueryable<ReadProjectDtos>> GetAllProjectsAsync();
        Task<ReadProjectDtos?> GetProjectByIdAsync(int id);
        Task<CreateProjectDtos> CreateProjectAsync(CreateProjectDtos createProjectDtos);
        Task UpdateProjectAsync(int id);
        Task DeleteProjectAsync(int id);
    }
}
