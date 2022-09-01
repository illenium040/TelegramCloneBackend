using TelegramCloneBackend.Database.Models;
using TelegramCloneBackend.Database.Models.DTO;

namespace TelegramCloneBackend.Database.Repositories.Base
{
    public interface IChatRepository
    {
        int GetMessagesCount(string id);
        Chat GetChat(string id);
        Message SendMessage(MessageToServerDTO message);
        Message GetLastMessageFromChat(string id);
    }
}
