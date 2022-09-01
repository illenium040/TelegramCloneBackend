using TelegramCloneBackend.Database.Contexts;
using TelegramCloneBackend.Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TelegramCloneBackend.Database.Repositories.Base;
using TelegramCloneBackend.Database.Models.DTO;

namespace TelegramCloneBackend.Database.Repositories
{
    public class ChatRepository : IChatRepository, IMessageRepository
    {
        private ChatContext _chatContext;
        public ChatRepository(ChatContext context)
        {
            _chatContext = context;
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

        public Message SendMessage(MessageToServerDTO message)
        {
            var chat = _chatContext.Chats.First(x => x.Id == message.ChatId);
            if (chat.Messages == null)
                chat.Messages = new List<Message>();
            var msg = new Message
            {
                Chat = chat,
                Content = message.Content,
                Created = DateTime.UtcNow,
                FromUserId = message.UserIdFrom,
                Id = Guid.NewGuid().ToString()
            };
            chat.Messages.Add(msg);
            _chatContext.SaveChanges();
            return msg;
        }
    }
}
