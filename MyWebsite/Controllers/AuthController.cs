using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebsite.Dtos.AuthDtos;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AuthController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

       
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _serviceManager.AuthService.LoginAsync(dto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return Unauthorized("yetkisiz giriş");

        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("refreshToken");
            return Ok(new { Message = "Başarıyla çıkış yapıldı." });
        }
    }
}
