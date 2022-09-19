using Microsoft.AspNetCore.Mvc;
using Database.Contexts;
using Database.Models;
using Database.Models.DTO;
using MediatR;
using MidiatRHandlers.Chat.GetWithMessages;
using MidiatRHandlers;
using MidiatRHandlers.Chat.GetChatList;
using MidiatRHandlers.Chat.Create;
using Microsoft.AspNetCore.Authorization;
using MidiatRHandlers.Chat.Delete;

namespace TGBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("messages/{chatId}")]
        public async Task<RequestResult<ChatDTO>> GetChatWithMessages(string chatId) => await _mediator.Send(new ChatWithMessagesQuery { ChatId = chatId });

        [HttpGet("list/{userId}")]
        public async Task<RequestResult<IEnumerable<ChatListUnit>>> GetChatList(string userId) => await _mediator.Send(new ChatListQuery { UserId = userId });

        [HttpPost("add")]
        public async Task<RequestResult<string>> Create(ChatCreationQuery query) => await _mediator.Send(query);

        [HttpPost("delete")]
        public async Task<RequestResult<string>> Delete(ChatDeletionQuery query) => await _mediator.Send(query);
    }
}
