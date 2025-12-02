using MyWebsite.Dtos.AuthDtos;
using MyWebsite.Entities;

namespace MyWebsite.Repository.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<IEnumerable<User>> GetAllUsers();

        Task CreateUser(User user);
        Task<User> GetUserById(int id);
        Task UpdateUser(User user);
        Task DeleteUser(User user);
        Task<User> GetLoginResult(LoginDto loginDto);
        Task<IEnumerable<User>> GetAllUsersForArticles();

    }
}
