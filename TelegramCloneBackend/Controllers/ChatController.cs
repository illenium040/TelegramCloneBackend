using Microsoft.AspNetCore.Mvc;
using DatabaseLayer.Contexts;
using DatabaseLayer.Models;
using DatabaseLayer.Models.DTO;
using MediatR;
using MidiatRHandlers.Chat.GetWithMessages;
using MidiatRHandlers;
using MidiatRHandlers.Chat.GetChatList;
using MidiatRHandlers.Chat.Create;
using Microsoft.AspNetCore.Authorization;
using MidiatRHandlers.Chat.Delete;
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
        public async Task<RequestResult<ChatDTO>> GetChatWithMessages(string chatId) => await _mediator.Send(new ChatWithMessagesQuery { ChatId = chatId });

        [HttpGet("list/{userId}")]
        public async Task<RequestResult<IEnumerable<ChatView>>> GetChatList(string userId) => await _mediator.Send(new ChatListQuery { UserId = userId });

        [HttpPost("add")]
        public async Task<RequestResult<string>> Create(ChatCreationQuery query) => await _mediator.Send(query);

        [HttpPost("delete")]
        public async Task<RequestResult> Delete(ChatDeletionQuery query) => await _mediator.Send(query);

        [HttpPost("archive")]
        public async Task<RequestResult> Archive([FromQuery] string userId, [FromQuery] string chatId)
        {
            return new RequestResult<bool>
            {
                Data = _userChat.ArchiveChat(chatId, userId),
                Status = System.Net.HttpStatusCode.OK,
                Succeeded = true
            };
        }
        [HttpPost("togglePin")]
        public async Task<RequestResult> TogglePin([FromQuery] string userId, [FromQuery] string chatId)
        {
            return new RequestResult<bool>
            {
                Data = _userChat.TogglePin(chatId, userId),
                Status = System.Net.HttpStatusCode.OK,
                Succeeded = true
            };
        }
        [HttpPost("toggleNotifications")]
        public async Task<RequestResult> ToggleNotifications([FromQuery] string userId, [FromQuery] string chatId)
        {
            return new RequestResult<bool>
            {
                Data = _userChat.ToggleNotifications(chatId, userId),
                Status = System.Net.HttpStatusCode.OK,
                Succeeded = true
            };
        }
        [HttpPost("block")]
        public async Task<RequestResult> Block([FromQuery] string userId, [FromQuery] string chatId)
        {
            return new RequestResult<bool>
            {
                Data = _userChat.BlockChat(chatId, userId),
                Status = System.Net.HttpStatusCode.OK,
                Succeeded = true
            };
        }
    }
}
