using DatabaseLayer.Models.DTO;
using MediatR;

namespace MidiatRHandlers.Chat.GetChatList
{
    public class ChatListQuery : IRequest<RequestResult<IEnumerable<ChatView>>>
    {
        public string UserId { get; set; }
    }
}
