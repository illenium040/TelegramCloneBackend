using Database.Models.DTO;
using MediatR;

namespace MidiatRHandlers.Chat.Create
{
    public class ChatCreationQuery : IRequest<RequestResult<string>>
    {
        public string WithUserId { get; set; }
        public string UserId { get; set; }
    }
}
