using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public UserController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public  async Task<IActionResult> GetAllUsers()
        {
            var users = await _manager.User.GetAllUsers();
            return Ok(users);
        }
    }
}
