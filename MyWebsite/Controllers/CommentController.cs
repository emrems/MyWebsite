using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebsite.Dtos.Comment;
using MyWebsite.Dtos.Error;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : BaseController
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
            return CreateResponse(result);
        }

        //[Authorize]
        //[HttpGet("{id}")]
        //public async Task<IActionResult> getAllComments(int id)
        //{
        //    var result = await _manager.CommentService.GetCommentById(id);
        //    return CreateResponse(result);
        //}

        [HttpPost("CreateComment")]
        public async Task<IActionResult> CreateComment(createCommentDto dto)
        {
            var result = await _manager.CommentService.CreateComment(dto);
            return CreateResponse(result);
        }
    }
}
