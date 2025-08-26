using Microsoft.EntityFrameworkCore;
using MyWebsite.Entities;
using MyWebsite.Repository.config;
using MyWebsite.Repository.Interfaces;

namespace MyWebsite.Repository.Concrate
{
    public class UserRepository : RepositoryBase<User>,IUserRepository
    {
        public UserRepository(MyWebSiteData dbContext) : base(dbContext)
        {
        }

        public Task CreateUser(User user)
        {
            return AddAsync(user);
        }

        public Task DeleteUser(User user)
        {
            return DeleteAsync(user);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await FindAll().ToListAsync();
        }

        public Task<User> GetUserById(int id)
        {
            return GetByIdAsync(id);
        }

        public Task UpdateUser(User user)
        {
            return UpdateAsync(user);
        }
    }
}
