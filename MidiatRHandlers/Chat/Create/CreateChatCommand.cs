using DatabaseLayer.Models.DTO;
using MediatR;

namespace CQRSLayer.Chat.Create
{
    public record CreateChatCommand(string WithUserId, string UserId) : ICommand<string>;
}
