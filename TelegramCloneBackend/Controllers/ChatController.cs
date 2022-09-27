using Microsoft.AspNetCore.Mvc;
using DatabaseLayer.Contexts;
using DatabaseLayer.Models;
using DatabaseLayer.Models.DTO;
using MediatR;
using CQRSLayer.Chat.GetWithMessages;
using CQRSLayer;
using CQRSLayer.Chat.GetChatList;
using CQRSLayer.Chat.Create;
using Microsoft.AspNetCore.Authorization;
using CQRSLayer.Chat.Delete;
using DatabaseLayer.Repositories.Base;

namespace TGBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserChatRepository _userChat;
        public ChatController(IMediator mediator, IUserChatRepository userChat)
        {
            _mediator = mediator;
            _userChat = userChat;
        }


        [HttpGet("messages/{chatId}")]
        public async Task<CommandResult<ChatDTO>> GetChatWithMessages(string chatId) => await _mediator.Send(new GetMessagesCommand(chatId));

        [HttpGet("list/{userId}")]
        public async Task<CommandResult<IEnumerable<ChatView>>> GetChatList(string userId) => await _mediator.Send(new GetChatListCommand(userId));

        [HttpPost("add")]
        public async Task<CommandResult<string>> Create(CreateChatCommand query) => await _mediator.Send(query);

        [HttpPost("delete")]
        public async Task<CommandResult> Delete(DeleteChatCommand query) => await _mediator.Send(query);

        [HttpPost("archive")]
        public async Task<CommandResult<bool>> Archive([FromQuery] string userId, [FromQuery] string chatId)
        {
            return new CommandResult<bool>
            {
                Data = _userChat.ArchiveChat(chatId, userId),
                Result = CommandResult.OK()
            };
        }
        [HttpPost("togglePin")]
        public async Task<CommandResult<bool>> TogglePin([FromQuery] string userId, [FromQuery] string chatId)
        {
            return new CommandResult<bool>
            {
                Data = _userChat.TogglePin(chatId, userId),
                Result = CommandResult.OK()
            };
        }
        [HttpPost("toggleNotifications")]
        public async Task<CommandResult<bool>> ToggleNotifications([FromQuery] string userId, [FromQuery] string chatId)
        {
            return new CommandResult<bool>
            {
                Data = _userChat.ToggleNotifications(chatId, userId),
                Result = CommandResult.OK()
            };
        }
        [HttpPost("block")]
        public async Task<CommandResult<bool>> Block([FromQuery] string userId, [FromQuery] string chatId)
        {
            return new CommandResult<bool>
            {
                Data = _userChat.BlockChat(chatId, userId),
                Result = CommandResult.OK()
            };
        }
    }
}
