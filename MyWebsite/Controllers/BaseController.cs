using Microsoft.AspNetCore.Mvc;
using MyWebsite.Dtos.Error;
using MyWebsite.Dtos.Response;

namespace MyWebsite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public  class BaseController:ControllerBase
    {
        protected IActionResult CreateResponse<T>(BaseResponse<T> response) where T : class
        {
            if (response.IsSuccess) return Ok(response);
            return response.ErrroCode switch
            {
                ErrorCodes.NotFound => NotFound(response),
                ErrorCodes.Unauthorized => Unauthorized(response),
               
                ErrorCodes.ValidationError => BadRequest(response),
            
                ErrorCodes.BadRequest => BadRequest(response),
                _ => BadRequest(response),
            };

        }
    }
}
