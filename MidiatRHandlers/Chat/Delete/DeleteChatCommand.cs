using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSLayer.Chat.Delete
{
    public record DeleteChatCommand(string UserId, string ChatId) : ICommand;
}
