using DatabaseLayer.Models;

namespace DatabaseLayer.Repositories.Base
{
    public interface IUserChatRepository
    {
        string? AddChat(string userId, string withUserId);
        IEnumerable<Chat> GetUserChatList(string userId);
        void RemoveChat(string chatId, string userId);
    }
}
