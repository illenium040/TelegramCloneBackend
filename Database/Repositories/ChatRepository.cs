using TelegramCloneBackend.Database.Contexts;
using TelegramCloneBackend.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace TelegramCloneBackend.Database.Repositories
{
    //FIRE!!!!!!!!!!!!!!!!!!!!!
    //(will be refactroed tommorow)
    public class ChatRepository
    {
        private ChatContext _chatContext;
        public ChatRepository(ChatContext context)
        {
            _chatContext = context;
        }

        public void SetHubConnection(string userId, string connectionId)
        {
            _chatContext.Users.Find(userId).HubConnections.Add(connectionId);
        }
        public void RemoveHubConnection(string userId, string connectionId)
        {
            _chatContext.Users.Find(userId).HubConnections.Remove(connectionId);
        }

        public User GetUser(string userId) => _chatContext.Users.Find(userId);
        public List<User> GetUsers() => _chatContext.Users.ToList();
        public List<Chat> GetUserChatList(string userId)
        {
            return _chatContext.Users
                .Include(x => x.Chats)
                .ThenInclude(x => x.Users)
                .FirstOrDefault(x => x.Id == userId)
                .Chats;
        }

        public Message GetLastMessageFromChat(string chatId)
        {
            var chat =  _chatContext.Chats
                .Include(x => x.Messages)
                .First(x => x.Id == chatId);
            return chat.Messages.OrderBy(x => x.Created).Last();
        }

        public Chat GetChat(string chatId)
        {
            return _chatContext.Chats.Include(x => x.Messages).First(x => x.Id == chatId);
        }

        public int GetMessagesCount(string chatId)
        {
            return _chatContext.Messages
                .Include(x => x.Chat)
                .Where(x => x.Chat.Id == chatId)
                .Count();
        }

        public Message SendMessage(string userId, string chatId, string message)
        {
            var chat = _chatContext.Chats.First(x => x.Id == chatId);
            if (chat.Messages == null)
                chat.Messages = new List<Message>();
            var msg = new Message
            {
                Chat = chat,
                Content = message,
                Created = DateTime.UtcNow,
                FromUserId = userId,
                Id = Guid.NewGuid().ToString()
            };
            chat.Messages.Add(msg);
            _chatContext.SaveChanges();
            return msg;
        }

        public string CreateChatBetweenUsers(string firstUserId, string secondUserId)
        {
            var user1 = _chatContext.Users.First(x => x.Id == firstUserId);
            var user2 = _chatContext.Users.First(x => x.Id == secondUserId);
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
            _chatContext.Chats.Add(chat);
            _chatContext.SaveChanges();
            return chat.Id;
        }

        public void AddUser(User user)
        {
            _chatContext.Users.Add(user);
            _chatContext.SaveChanges();
        }
    }
}
