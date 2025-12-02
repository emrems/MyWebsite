using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyWebsite.Dtos.Error;
using MyWebsite.Dtos.UserDtos;
using MyWebsite.Entities;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : BaseController
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
            return CreateResponse(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto user)
        {
          
            var result =await _manager.UserService.CreateUser(user);
            return CreateResponse(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result =await _manager.UserService.DeleteUser(id);
            return CreateResponse(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _manager.UserService.GetUserById(id);
            return CreateResponse(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser( int id,[FromBody] UpdateUserDto user)
        {
            user.Id = id;
            var result =await _manager.UserService.UpdateUser(user);
            return CreateResponse(result);
        }
    }
}
