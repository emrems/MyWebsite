using MyWebsite.Dtos.ProjectDtos;
using MyWebsite.Dtos.Response;

namespace MyWebsite.Service.İnterfaces
{
    public interface IProjectService
    {
        Task<BaseResponse<IQueryable<ReadProjectDtos>>> GetAllProjectsAsync();
        Task<BaseResponse<ReadProjectDtos?>> GetProjectByIdAsync(int id);
        Task<BaseResponse<Object>> CreateProjectAsync(CreateProjectDtos createProjectDtos);
        Task<BaseResponse<object>> UpdateProjectAsync(int id, UpdateProjectDtos dtos);
        Task<BaseResponse<object>> DeleteProjectAsync(int id);
    }
}
