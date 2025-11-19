using Microsoft.AspNetCore.Mvc;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Controllers
{
    [ApiController]
    [Route("api/skills")]
    public class SkillsController : BaseController
    {
        private readonly IServiceManager _manager;

        public SkillsController(IServiceManager manager)
        {
            _manager = manager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSkills()
        {
            var result = await _manager.SkillService.GetAllSkills();
            return CreateResponse(result);
        }
    }
}
