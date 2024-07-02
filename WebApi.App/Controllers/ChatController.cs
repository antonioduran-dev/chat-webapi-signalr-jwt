using DB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApi.App.DTOs;
using WebApi.App.Hubs;
using WebApi.App.Services;

namespace WebApi.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        // create the chat service interface and Hub context interface. 
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IChatService chatService, IHubContext<ChatHub> hubContext)
        {
            _chatService = chatService;
            _hubContext = hubContext;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MessageDTO>))]
        [HttpGet("messages")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessages()
        {
            IEnumerable<MessageDTO> messages = await _chatService.GetAll();

            return Ok(messages);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("send")]
        [Authorize]
        public async Task<ActionResult> SendMessage([FromBody] MessageDTO message)
        {
            var userClaims = User.Claims;
            // get the userId by User authorized.
            var userId = userClaims.FirstOrDefault(u => u.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            message.UserId = Convert.ToInt32(userId);
            var response = await _chatService.Post(message);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", response.User.UserName, response.Text, response.TimeStamp);

            return Ok();
        }
    }
}
