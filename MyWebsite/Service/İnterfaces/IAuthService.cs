using MyWebsite.Dtos.AuthDtos;

namespace MyWebsite.Service.İnterfaces
{
    public interface IAuthService
    {
        Task<LoginResultDto> LoginAsync(LoginDto loginDto);
    }
}
