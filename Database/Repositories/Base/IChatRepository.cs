using Database.Models;
using Database.Models.DTO;

namespace Database.Repositories.Base
{
    public interface IChatRepository
    {
        int GetMessagesCount(string id);
        int GetUnreadMessagesCount(string chatId, string userId);
        Chat GetChat(string id);
        Message SendMessage(MessageDTO message);
        Message GetLastMessageFromChat(string id);
        void ReadMessages(IEnumerable<string> messages, string chatId);
    }
}
