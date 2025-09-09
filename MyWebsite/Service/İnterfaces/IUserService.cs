using MyWebsite.Dtos.AuthDtos;
using MyWebsite.Dtos.UserDtos;
using MyWebsite.Entities;

namespace MyWebsite.Service.İnterfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task CreateUser(CreateUserDto user);
        Task<User> GetUserById(int id);
        Task UpdateUser(UpdateUserDto user);
        Task DeleteUser(int id);

    }
}
