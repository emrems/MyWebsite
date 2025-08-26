using MyWebsite.Entities;

namespace MyWebsite.Service.İnterfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task CreateUser(User user);
        Task<User> GetUserById(int id);
        Task UpdateUser(User user);
        Task DeleteUser(User user);

    }
}
