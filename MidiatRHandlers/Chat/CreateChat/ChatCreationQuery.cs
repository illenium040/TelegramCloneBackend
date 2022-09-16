using Database.Models.DTO;
using MediatR;

namespace MidiatRHandlers.Chat.CreateChat
{
    public class ChatCreationQuery : IRequest<RequestResult<string>>
    {
        public string TargetUser { get; set; }
        public string CurrentUser { get; set; }
    }
}
