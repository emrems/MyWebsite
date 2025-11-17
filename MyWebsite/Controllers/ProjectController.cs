using Microsoft.AspNetCore.Mvc;
using MyWebsite.Dtos.Error;
using MyWebsite.Dtos.ProjectDtos;
using MyWebsite.Entities;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Controllers
{

    [Route("api/Projects")]
    [ApiController]
    public class ProjectController : BaseController
    {
        private readonly IServiceManager _manager;
        public ProjectController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _manager.ProjectService.GetAllProjectsAsync();
            return CreateResponse(projects);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _manager.ProjectService.GetProjectByIdAsync(id);
            return CreateResponse(project);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDtos project)
        {
            var result =await _manager.ProjectService.CreateProjectAsync(project);
            return CreateResponse(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject([FromRoute] int id, [FromBody] UpdateProjectDtos dto)
        {
            var result =await _manager.ProjectService.UpdateProjectAsync(id,dto);
            return CreateResponse(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var result =await _manager.ProjectService.DeleteProjectAsync(id);
            return CreateResponse(result);
        }

    }
}
