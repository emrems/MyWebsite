using Microsoft.AspNetCore.Mvc;
using MyWebsite.Dtos.MessageDtos;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Controllers
{
    [ApiController]
    [Route("api/message")]
    public class MessageController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public MessageController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageDtos message)
        {
            await _serviceManager.MessageService.CreateMessageAsync(message);
            return Ok("Mesaj başarılı bir şekilde gönderildi");
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = await _serviceManager.MessageService.GetAllMessagesAsync();
            return Ok(messages);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            await _serviceManager.MessageService.DeleteMessageAsync(id);
            return Ok("Mesaj başarılı bir şekilde silindi");
        }
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetMessageById(int id)
        {
            var message = await _serviceManager.MessageService.GetMessageByIdAsync(id);
            return Ok(message);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateMessage(int id, [FromBody] MessageDtos message)
        {
            await _serviceManager.MessageService.UpdateMessageAsync(id, message);
            return Ok("Mesaj başarılı bir şekilde güncellendi");
        }
    }
}
