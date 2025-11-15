using MyWebsite.Dtos.AuthDtos;
using MyWebsite.Dtos.Response;

namespace MyWebsite.Service.İnterfaces
{
    public interface IAuthService
    {
        Task<BaseResponse<LoginResultDto>> LoginAsync(LoginDto loginDto);
    }
}
