using Microsoft.AspNetCore.Mvc;
using MyWebsite.Dtos.Error;
using MyWebsite.Dtos.MessageDtos;
using MyWebsite.Entities;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Controllers
{
    [ApiController]
    [Route("api/message")]
    public class MessageController : BaseController
    {
        private readonly IServiceManager _serviceManager;
        public MessageController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageDtos message)
        {
            var result =await _serviceManager.MessageService.CreateMessageAsync(message);
            return CreateResponse(result);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = await _serviceManager.MessageService.GetAllMessagesAsync();
            return CreateResponse(messages);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var result =await _serviceManager.MessageService.DeleteMessageAsync(id);
            return CreateResponse(result);
        }
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetMessageById(int id)
        {
            var message = await _serviceManager.MessageService.GetMessageByIdAsync(id);
            return CreateResponse(message);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateMessage(int id, [FromBody] MessageDtos message)
        {
            var result =await _serviceManager.MessageService.UpdateMessageAsync(id, message);
            return CreateResponse(result);
        }
    }
}
