using Microsoft.AspNetCore.Mvc;
using MyWebsite.Dtos.MediaDtos;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediasController : BaseController
    {
        private readonly IServiceManager _manager;

        public MediasController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpPost("createMedia")]
        public async Task<IActionResult> createMedia([FromForm] CreateMediaDto dto) 
        {
            var result = await _manager.MediaService.createMedia(dto);
            return CreateResponse(result);
        }
    }
}
