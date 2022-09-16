using Database.Models.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiatRHandlers.Chat.GetWithMessages
{
    public class ChatWithMessagesQuery : IRequest<RequestResult<ChatDTO>>
    {
        public string ChatId { get; set; }
    }
}
