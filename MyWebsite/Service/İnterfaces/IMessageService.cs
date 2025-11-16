using MyWebsite.Dtos.MessageDtos;
using MyWebsite.Dtos.Response;

namespace MyWebsite.Service.İnterfaces
{
    public interface IMessageService
    {
        Task<BaseResponse<IQueryable<MessageDtos>>> GetAllMessagesAsync();
        Task<BaseResponse<MessageDtos>> GetMessageByIdAsync(int id);
        Task<BaseResponse<object>> CreateMessageAsync(MessageDtos messageDto);
        Task<BaseResponse<object>> DeleteMessageAsync(int id);
        Task<BaseResponse<object>> UpdateMessageAsync(int id, MessageDtos dto);
    }
}
