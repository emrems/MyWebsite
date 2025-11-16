using MyWebsite.Dtos.AuthDtos;
using MyWebsite.Dtos.Response;
using MyWebsite.Dtos.UserDtos;
using MyWebsite.Entities;

namespace MyWebsite.Service.İnterfaces
{
    public interface IUserService
    {
        Task<BaseResponse<IEnumerable<ReadUserDtos>>> GetAllUsers();
        Task<BaseResponse<object>> CreateUser(CreateUserDto user);
        Task<BaseResponse<ReadUserDtos>> GetUserById(int id);
        Task<BaseResponse<object>> UpdateUser(UpdateUserDto user);
        Task<BaseResponse<object>> DeleteUser(int id);

    }
}
