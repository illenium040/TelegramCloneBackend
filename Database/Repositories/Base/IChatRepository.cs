using Database.Models;
using Database.Models.DTO;
using DatabaseLayer.Models;

namespace Database.Repositories.Base
{
    public interface IChatRepository
    {
        int GetMessagesCount(string id);
        int GetUnreadMessagesCount(string chatId, string userId);
        IEnumerable<Message> GetMessages(string id);
        Message GetLastMessageFromChat(string id);
        void ReadMessages(IEnumerable<string> messages, string chatId);
    }
}
