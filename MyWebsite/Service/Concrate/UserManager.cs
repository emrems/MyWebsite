using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyWebsite.Dtos.ArticleDtos;
using MyWebsite.Dtos.AuthDtos;
using MyWebsite.Dtos.Error;
using MyWebsite.Dtos.Response;
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
        private readonly IValidator<UpdateUserDto> _userUpdateValidator;
        private readonly IValidator<CreateUserDto> _userCreateValidator;
        public UserManager(IRepositoryManager manager, IConfiguration configuration, IValidator<UpdateUserDto> userUpdateValidator , IValidator<CreateUserDto> userCreateValidator)
        {
            _manager = manager;
            _configuration = configuration;
            _userUpdateValidator = userUpdateValidator;
            _userCreateValidator = userCreateValidator;
        }

        public async Task<BaseResponse<object>> CreateUser(CreateUserDto user)
        {
            var validateResult = await _userCreateValidator.ValidateAsync(user);
            if (!validateResult.IsValid)
            {
                return new BaseResponse<object>
                {
                    IsSuccess = false,
                    Data = null,
                    ErrroCode = ErrorCodes.ValidationError,
                    Message = validateResult.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                };
            }
            var userCreate = new User
            {
                Username = user.Username,
                SurName=user.SurName,
                Email = user.Email,
                PasswordHash = user.Password,
                CreatedDate = DateTime.Now,
                Role = "User"
            };
            await _manager.UserRepository.CreateUser(userCreate);
            await _manager.SaveAsync();
            return new BaseResponse<object>
            {
                IsSuccess = false,
                Data = null,
                ErrroCode = null,
                Message = "kullanıcı başarılı bir şekilde eklendi"
            };
        }

        public async Task<BaseResponse<object>> DeleteUser(int id)
        {
            var userDb = await _manager.UserRepository.GetUserById(id);

            if(userDb is null)
            {
                throw new NotFoundException("silinecek user bulunamadı");
            }
            await _manager.UserRepository.DeleteUser(userDb);
            await _manager.SaveAsync();
            return new BaseResponse<object>
            {
                IsSuccess = true,
                Data = null,
                ErrroCode=null,
                Message="user başarılı bir şekilde silinmiştir"
            };


        }

        public async Task<BaseResponse<IEnumerable<ReadUserDtos>>> GetAllUsers()
        {
            var users= await _manager.UserRepository.GetAllUsers();
            var userList = users.Select(user => new ReadUserDtos
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Articles = user.Articles?.Select(art => new ReadArticleDtos
                {
                    Id = art.Id,
                    Title = art.Title,
                    Content = art.Content,
                    Slug = art.Slug,
                    CreatedDate = art.CreatedDate
                }).ToList()
            }).ToList();
            return new BaseResponse<IEnumerable<ReadUserDtos>>
            {
                IsSuccess = true,
                Data = userList,
                Message = "userlar başarılı bir şekilde çekildi",
                ErrroCode = null
            };
        }

        

        public async Task<BaseResponse<ReadUserDtos>> GetUserById(int id)
        {
            var user = await _manager.UserRepository.GetUserById(id);
            if (user == null)
                throw new NotFoundException($"{id}' li kullanıcı bulunamadı");

            var userDto = new ReadUserDtos
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Articles = user.Articles.Select(art => new ReadArticleDtos
                {
                    Id = art.Id,
                    Title = art.Title,
                    Content = art.Content,
                    Slug = art.Slug,
                    CreatedDate = art.CreatedDate
                }).ToList()
            };
            return new BaseResponse<ReadUserDtos>
            {
                IsSuccess = true,
                Data = userDto,
                Message = "user başarılı bir şekilde çekildi",
                ErrroCode = null
            };
        }




        public async Task<BaseResponse<object>> UpdateUser(UpdateUserDto userDto)
        {
            var validatorResult = await _userUpdateValidator.ValidateAsync(userDto);

            if(!validatorResult.IsValid)
            {
                return new BaseResponse<object>
                {
                    IsSuccess = false,
                    Data = null,
                    ErrroCode = ErrorCodes.ValidationError,
                    Message = validatorResult.Errors.Select(er => er.ErrorMessage).FirstOrDefault()
                };
            }
            var userDb = await _manager.UserRepository.GetUserById(userDto.Id);
            if(userDb is null)
            {
                throw new NotFoundException("güncellenecek user bulunamadı");
            }
            userDb.Username = userDto.UserName;
            userDb.Email = userDto.UserEmail;
            await _manager.UserRepository.UpdateUser(userDb);
            await _manager.SaveAsync();
            return new BaseResponse<object>
            {
                IsSuccess = true,
                Data = null,
                Message = "user başarılı bir şekilde güncellendi",
                ErrroCode = null
            };
        }
    }
}
