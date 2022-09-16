using Database.Models.DTO;
using MediatR;

namespace MidiatRHandlers.Chat.GetChatList
{
    public class ChatListQuery : IRequest<RequestResult<IEnumerable<ChatListUnit>>>
    {
        public string UserId { get; set; }
    }
}
