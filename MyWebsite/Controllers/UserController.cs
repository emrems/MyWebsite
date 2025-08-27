using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyWebsite.Dtos.UserDtos;
using MyWebsite.Entities;
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
            var users = await _manager.UserService.GetAllUsers();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto user)
        {
          
            await _manager.UserService.CreateUser(user);
            return StatusCode(201, "kullanıcı başarıyla eklendi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _manager.UserService.DeleteUser(id);
            return Ok("kullanıcı başarıyla silindi.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _manager.UserService.GetUserById(id);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser( [FromBody] UpdateUserDto user)
        {
            
            await _manager.UserService.UpdateUser(user);
            return Ok("kullanıcı başarıyla güncellendi.");
        }
    }
}
