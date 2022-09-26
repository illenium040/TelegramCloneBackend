using DatabaseLayer.Models;

namespace DatabaseLayer.Repositories.Base
{
    public interface IUserChatRepository
    {
        IEnumerable<ChatToUser> GetUserChatList(string userId);
        string? AddChat(string userId, string withUserId);
        void RemoveChat(string chatId, string userId);

        bool ArchiveChat(string chatId, string userId);
        bool TogglePin(string chatId, string userId);
        bool ToggleNotifications(string chatId, string userId);
        void AddToFolder(string folderId, string chatId, string userId);
        void RemoveFromFolder(string folderId, string chatId, string userId);
        bool BlockChat(string chatId, string userId);

    }
}
