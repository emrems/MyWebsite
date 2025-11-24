using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebsite.Dtos.Experince;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperinceController : BaseController
    {
        private readonly IServiceManager _manager;

        public ExperinceController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> getAllExperince()
        {
            var result = await _manager.ExperinceService.GetAllExperinceAsync();
            return CreateResponse(result);
        }

        [HttpGet("id")]
        public async Task<IActionResult> getAllExperince(int id)
        {
            var result = await _manager.ExperinceService.GetExperinceById(id);
            return CreateResponse(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> getAllExperince(CreateExperinceDtos dto)
        {
            var result = await _manager.ExperinceService.createExperienceAsync(dto);
            return CreateResponse(result);
        }
    }
}
