using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiatRHandlers.Chat.Delete
{
    public class ChatDeletionQuery : IRequest<RequestResult<string>>
    {
        public string UserId { get; set; }
        public string ChatId { get; set; }
    }
}
