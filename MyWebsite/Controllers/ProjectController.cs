using Microsoft.AspNetCore.Mvc;
using MyWebsite.Dtos.Error;
using MyWebsite.Dtos.ProjectDtos;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Controllers
{

    [Route("api/Projects")]
    [ApiController]
    public class ProjectController : ControllerBase
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
            return Ok(projects);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _manager.ProjectService.GetProjectByIdAsync(id);
            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDtos project)
        {
            var result =await _manager.ProjectService.CreateProjectAsync(project);
            if (!result.IsSuccess)
            {
                if(result.ErrroCode == ErrorCodes.NotFound)
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject([FromRoute] int id, [FromBody] UpdateProjectDtos dto)
        {
            var result =await _manager.ProjectService.UpdateProjectAsync(id,dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            await _manager.ProjectService.DeleteProjectAsync(id);
            return Ok("Proje başarıyla silindi.");
        }

    }
}
