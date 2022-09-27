using DatabaseLayer.Models.DTO;
using MediatR;

namespace CQRSLayer.Chat.GetChatList
{
    public record GetChatListCommand(string UserId) : ICommand<IEnumerable<ChatView>>;
}
