using DatabaseLayer.Models;
using DatabaseLayer.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Repositories.Base
{
    public interface IMessagingRepository
    {
        void ReadMessages(IEnumerable<string> messages, string chatId);
        Message SendToUser(MessageDTO message);
        Message SendMessage(MessageDTO message, out ChatView receiver);
    }
}
