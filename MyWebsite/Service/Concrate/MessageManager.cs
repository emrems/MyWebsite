using Microsoft.EntityFrameworkCore;
using MyWebsite.Dtos.MessageDtos;
using MyWebsite.Entities;
using MyWebsite.Exceptions;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Service.Concrate
{
    public class MessageManager : IMessageService
    {
        private readonly IRepositoryManager _repositoryManager;
        public MessageManager(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task CreateMessageAsync(MessageDtos messageDto)
        {
            var createdMessage = new Message
            {
                Name = messageDto.Name,
                Email = messageDto.Email,
                Content = messageDto.Content
            };
            await _repositoryManager.MessageRepository.AddAsync(createdMessage);
            await _repositoryManager.SaveAsync();
            
        }

        public async Task DeleteMessageAsync(int id)
        {
            var message = await  _repositoryManager.MessageRepository.GetByIdAsync(id);
            if (message == null)
            {
                throw new NotFoundException("Message not found");
            }
            await _repositoryManager.MessageRepository.DeleteAsync(message);
            await _repositoryManager.SaveAsync();
        }

        public async Task<IQueryable<MessageDtos>> GetAllMessagesAsync()
        {
            var messages = await _repositoryManager.MessageRepository.FindAll().ToListAsync();
            var messageDtos = messages.Select(m => new MessageDtos
            {
                Name = m.Name,
                Email = m.Email,
                Content = m.Content
            }).AsQueryable();
            return messageDtos;

        }

        public async Task<MessageDtos> GetMessageByIdAsync(int id)
        {
            var message = await _repositoryManager.MessageRepository.GetByIdAsync(id);
            if (message == null)
            {
                throw new NotFoundException("Message not found");
            }
            var messageDto = new MessageDtos
            {
                Name = message.Name,
                Email = message.Email,
                Content = message.Content
            };
            return messageDto;

            
        }

        public async Task UpdateMessageAsync(int id, MessageDtos dto)
        {
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

        }
    }
}
