using Microsoft.EntityFrameworkCore;
using TelegramCloneBackend.Database.Contexts;
using TelegramCloneBackend.Database.Models;
using TelegramCloneBackend.Database.Repositories.Base;

namespace TelegramCloneBackend.Database.Repositories
{
    public class UserRepository : IUserRepository, IConnectionRepository
    {
        private UserContext _userContext;
        public UserRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        public void Add(User user)
        {
            _userContext.Users.Add(user);
            _userContext.SaveChanges();
        }

        public string CreateChatBetweenUsers(string firstUserId, string secondUserId)
        {
            var user1 = _userContext.Users.First(x => x.Id == firstUserId);
            var user2 = _userContext.Users.First(x => x.Id == secondUserId);
            var chat = new Chat()
            {
                Id = Guid.NewGuid().ToString(),
                Users = new List<User>() { user1, user2 }
            };
            if (user1.Chats == null)
                user1.Chats = new List<Chat>();
            user1.Chats.Add(chat);
            if (user2.Chats == null)
                user2.Chats = new List<Chat>();
            user2.Chats.Add(chat);
            _userContext.SaveChanges();
            return chat.Id;
        }

        public User Get(string id)
        {
            return _userContext.Users
                .SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<Chat> GetUserChatList(string id)
        {
            return _userContext.Users
                .Include(x => x.Chats)
                .ThenInclude(x => x.Users)
                .FirstOrDefault(x => x.Id == id)
                .Chats;
        }

        public IEnumerable<Connection> GetUserConnections(string id)
        {
            return _userContext.Users
                .Include(x => x.Connections)
                .SingleOrDefault(x => x.Id == id)
                .Connections;
        }

        public IEnumerable<User> GetUsers()
        {
            return _userContext.Users.OrderBy(x => x.Name).ToList();
        }


        public void OnConnect(string userId, string connectionId, string userAgent)
        {
            var user = _userContext.Users
                .Include(x => x.Connections)
                .SingleOrDefault(x => x.Id == userId);
            if (user.Connections == null)
                user.Connections = new List<Connection>();

            var connection = user.Connections
                .Where(c => c.UserAgent == userAgent)
                .FirstOrDefault();

            if (connection == null)
            {
                connection = new Connection();
                connection.UserAgent = userAgent;
                user.Connections.Add(connection);
            }

            connection.ConnectionID = connectionId;
            connection.Connected = true;

            _userContext.SaveChanges();
        }

        public void OnDisconnect(string userId, string userAgent)
        {
            throw new NotImplementedException();
        }
    }
}
