using System.Collections;
using TelegramCloneBackend.Database.Models;

namespace TelegramCloneBackend.Database.Repositories.Base
{
    public interface IUserRepository
    {
        void Add(User user);
        User Get(string id);
        IEnumerable<User> GetUsers();
        IEnumerable<Connection> GetUserConnections(string id);
        IEnumerable<Chat> GetUserChatList(string id);
        string CreateChatBetweenUsers(string firstUserId, string secondUserId);
    }
}
