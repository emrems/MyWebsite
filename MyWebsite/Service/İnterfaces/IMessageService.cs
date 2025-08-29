using MyWebsite.Dtos.MessageDtos;

namespace MyWebsite.Service.İnterfaces
{
    public interface IMessageService
    {
        Task<IQueryable<MessageDtos>> GetAllMessagesAsync();
        Task<MessageDtos> GetMessageByIdAsync(int id);
        Task CreateMessageAsync(MessageDtos messageDto);
        Task DeleteMessageAsync(int id);
        Task UpdateMessageAsync(int id, MessageDtos dto);
    }
}
