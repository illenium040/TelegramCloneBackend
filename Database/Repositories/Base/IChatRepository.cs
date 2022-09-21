using DatabaseLayer.Models;
using DatabaseLayer.Models.DTO;
using DatabaseLayer.Models;
using DatabaseLayer.Repositories.Base;

namespace DatabaseLayer.Repositories.Base
{
    public interface IChatRepository : IRepository<Chat>
    {
        int GetMessagesCount(string id);
        int GetUnreadMessagesCount(string chatId, string userId);
        IEnumerable<Message> GetMessages(string id);
        Message GetLastMessageFromChat(string id);
    }
}
