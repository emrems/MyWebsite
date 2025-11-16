using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyWebsite.Dtos.Error;
using MyWebsite.Dtos.MessageDtos;
using MyWebsite.Dtos.Response;
using MyWebsite.Entities;
using MyWebsite.Exceptions;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Service.Concrate
{
    public class MessageManager : IMessageService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IValidator<MessageDtos> _messageValidator;
        public MessageManager(IRepositoryManager repositoryManager, IValidator<MessageDtos> messageValidator)
        {
            _repositoryManager = repositoryManager;
            _messageValidator = messageValidator;
        }

        public async Task<BaseResponse<object>> CreateMessageAsync(MessageDtos messageDto)
        {
            var resultValidator = await _messageValidator.ValidateAsync(messageDto);
            if (!resultValidator.IsValid)
            {
                return new BaseResponse<object>
                {
                    IsSuccess = false,
                    Message =  resultValidator.Errors.Select(err => err.ErrorMessage).FirstOrDefault(),
                    Data = null,
                    ErrroCode = ErrorCodes.ValidationError
                };
            }
            var createdMessage = new Message
            {
                Name = messageDto.Name,
                Email = messageDto.Email,
                Content = messageDto.Content
            };
            await _repositoryManager.MessageRepository.AddAsync(createdMessage);
            await _repositoryManager.SaveAsync();
            return new BaseResponse<object>
            {
                IsSuccess = true,
                ErrroCode = null,
                Message = "mesajlar çekildi",
                Data = createdMessage
            };

            
        }

        public async Task<BaseResponse<object>> DeleteMessageAsync(int id)
        {
            var message = await  _repositoryManager.MessageRepository.GetByIdAsync(id);
            if (message == null)
            {
                throw new NotFoundException("Message not found");
            }
            await _repositoryManager.MessageRepository.DeleteAsync(message);
            await _repositoryManager.SaveAsync();
            return new BaseResponse<object>
            {
                IsSuccess = true,
                ErrroCode = null,
                Message = "categori başarıl bir şekilde silindi",
                Data = null
            };
        }

        public async Task<BaseResponse<IQueryable<MessageDtos>>> GetAllMessagesAsync()
        {
            var messages = await _repositoryManager.MessageRepository.FindAll().ToListAsync();
           
            var messageDtos = messages.Select(m => new MessageDtos
            {
                Name = m.Name,
                Email = m.Email,
                Content = m.Content
            }).AsQueryable();
            return new BaseResponse<IQueryable<MessageDtos>>
            {
                IsSuccess = true,
                Data = messageDtos,
                Message = "mesajlar başarılı bir şekilde çekildi",
                ErrroCode = null
            };

        }

        public async Task<BaseResponse<MessageDtos>> GetMessageByIdAsync(int id)
        {
            var message = await _repositoryManager.MessageRepository.GetByIdAsync(id);
            if (message == null)
            {
                throw new NotFoundException("Message bulunamadı");
            }
            var messageDto = new MessageDtos
            {
                Name = message.Name,
                Email = message.Email,
                Content = message.Content
            };
            return new BaseResponse<MessageDtos>
            {
                IsSuccess = true,
                Message = "mesaj geldi",
                Data = messageDto,
                ErrroCode = null
            };

            
        }

        public async Task<BaseResponse<object>> UpdateMessageAsync(int id, MessageDtos dto)
        {
            var validateResult =await _messageValidator.ValidateAsync(dto);
            if (!validateResult.IsValid)
            {
                return new BaseResponse<object>
                {
                    IsSuccess = false,
                    Data = null,
                    ErrroCode = ErrorCodes.ValidationError,
                    Message = validateResult.Errors.Select(err => err.ErrorMessage).FirstOrDefault()
                };
            }
            var message = await _repositoryManager.MessageRepository.GetByIdAsync(id);
            if (message == null)
            {
                throw new NotFoundException("Message not found");
            }
            message.Content = dto.Content;
            message.Name = dto.Name;
            message.Email = dto.Email;
            await _repositoryManager.MessageRepository.UpdateAsync(message);
            await _repositoryManager.SaveAsync();
            return new BaseResponse<object>
            {
                IsSuccess = true,
                Data = null,
                ErrroCode = null,
                Message = "başarılı bir şekilde güncellendi"
            };

        }
    }
}
