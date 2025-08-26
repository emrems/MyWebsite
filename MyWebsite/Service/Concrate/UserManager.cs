using Microsoft.EntityFrameworkCore;
using MyWebsite.Entities;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Service.Concrate
{
    public class UserManager : IUserService
    {
        private readonly IRepositoryManager _manager;

        public UserManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public async Task CreateUser(User user)
        {
           await _manager.User.CreateUser(user);
            await _manager.SaveAsync();
        }

        public async Task DeleteUser(User user)
        {
            var userDb = await _manager.User.GetUserById(user.Id);

            if(userDb is null)
            {
                throw new Exception("silinecek user bulunamadı");
            }
            await _manager.User.DeleteUser(user);
            await _manager.SaveAsync();

        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _manager.User.GetAllUsers();
        }

        public Task<User> GetUserById(int id)
        {
            var user = _manager.User.GetUserById(id);
            if(user is null)
            {
                throw new Exception();
            }
            return user;
        }



        public async Task UpdateUser(User user)
        {
            if(user is null)
            {
                throw new Exception("user boş olamaz");
            }
            var userDb = _manager.User.GetUserById(user.Id);
            if(userDb is null)
            {
                throw new Exception("güncellenecek user bulunamadı");
            }
             await _manager.User.UpdateUser(user);
        }
    }
}
