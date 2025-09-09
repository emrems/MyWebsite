using Microsoft.EntityFrameworkCore;
using MyWebsite.Dtos.AuthDtos;
using MyWebsite.Dtos.UserDtos;
using MyWebsite.Entities;
using MyWebsite.Exceptions;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Service.Concrate
{
    public class UserManager : IUserService
    {
        private readonly IRepositoryManager _manager;
        private readonly IConfiguration _configuration;

        public UserManager(IRepositoryManager manager, IConfiguration configuration)
        {
            _manager = manager;
            _configuration = configuration;
        }

        public async Task CreateUser(CreateUserDto user)
        {
            var userCreate = new User
            {
                Username = user.Username,
                Email = user.Email,
                PasswordHash = user.Password,
                CreatedDate = DateTime.Now,
                Role = "User"
            };
            await _manager.UserRepository.CreateUser(userCreate);
            await _manager.SaveAsync();
        }

        public async Task DeleteUser(int id)
        {
            var userDb = await _manager.UserRepository.GetUserById(id);

            if(userDb is null)
            {
                throw new NotFoundException("silinecek user bulunamadı");
            }
            await _manager.UserRepository.DeleteUser(userDb);
            await _manager.SaveAsync();

        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _manager.UserRepository.GetAllUsers();
        }

        

        public async Task<User> GetUserById(int id)
        {
            var user = await _manager.UserRepository.GetUserById(id);
            if (user == null)
                throw new NotFoundException($"{id}' li kullanıcı bulunamadı");

            return user;
        }




        public async Task UpdateUser(UpdateUserDto userDto)
        {
            if(userDto is null)
            {
                throw new Exception("user boş olamaz");
            }
            var userDb = await _manager.UserRepository.GetUserById(userDto.Id);
            if(userDb is null)
            {
                throw new NotFoundException("güncellenecek user bulunamadı");
            }
            userDb.Id = userDto.Id;
            userDb.Username = userDto.UserName;
            userDb.Email = userDto.UserEmail;
            userDb.PasswordHash = userDto.Password;
           


            await _manager.UserRepository.UpdateUser(userDb);
            await _manager.SaveAsync();
        }
    }
}
