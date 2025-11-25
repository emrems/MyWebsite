using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebsite.Dtos.ArticleLikeDtos;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Controllers
{
    [ApiController]
    [Route("api/ArticleLike")]
    public class ArticleLikeController : BaseController
    {
        private readonly IServiceManager _manager;

        public ArticleLikeController(IServiceManager manager)
        {
            _manager = manager;
        }

        [Authorize]
        [HttpPost("createArticleLike")]
        public async Task<IActionResult> createArticliLike(CreateArticleLikeDto dto)
        {
         
            var result = await _manager.ArticleLikeService.ToggleLikeAsync(dto);
            return CreateResponse(result);
        }

    }
}
