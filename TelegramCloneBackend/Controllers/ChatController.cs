using Microsoft.AspNetCore.Mvc;
using Database.Contexts;
using Database.Models;
using Database.Models.DTO;
using Database.Repositories;
using MediatR;
using MidiatRHandlers.Chat.GetWithMessages;
using MidiatRHandlers;
using MidiatRHandlers.Chat.GetChatList;
using MidiatRHandlers.Chat.CreateChat;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<RequestResult<ChatDTO>> GetChatWithMessages(string chatId)
        {
            return await _mediator.Send(new ChatWithMessagesQuery { ChatId = chatId});
        }


        [HttpGet("list/{userId}")]
        public async Task<RequestResult<IEnumerable<ChatListUnit>>> GetChatList(string userId)
        {
            return await _mediator.Send(new ChatListQuery { UserId = userId});
        }


        [HttpPost("addchat")]
        public async Task<RequestResult<string>> CreateChat(ChatCreationQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}
