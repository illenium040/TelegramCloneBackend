using DatabaseLayer.Models.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSLayer.Chat.GetWithMessages
{
    public record GetMessagesCommand(string ChatId) : ICommand<ChatDTO>;
}
