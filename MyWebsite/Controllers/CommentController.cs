using Microsoft.AspNetCore.Mvc;
using MyWebsite.Dtos.Comment;
using MyWebsite.Dtos.Error;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public CommentController(IServiceManager manager)
        {
            _manager = manager;

        }

        [HttpGet("gettAllComment")]
        public async Task<IActionResult> getAllComments()
        {
            var result = await _manager.CommentService.gettAllComment();
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

        [HttpPost("CreateComment")]
        public async Task<IActionResult> CreateComment(createCommentDto dto)
        {
            var result = await _manager.CommentService.CreateComment(dto);
            if (!result.IsSuccess)
            {
                if (result.ErrroCode == ErrorCodes.NotFound)
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
