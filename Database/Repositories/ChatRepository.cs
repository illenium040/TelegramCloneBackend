using Database.Contexts;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Database.Repositories.Base;
using Database.Models.DTO;

namespace Database.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private ChatContext _chatContext;
        public ChatRepository(ChatContext context)
        {
            _chatContext = context;
        }

        public Message? GetLastMessageFromChat(string chatId)
        {
            var chat =  _chatContext.Chats
                .Include(x => x.Messages)
                .First(x => x.Id == chatId);
            if(chat.Messages.Count == 0) return null;
            return chat.Messages.OrderBy(x => x.Created)?.Last();
        }

        public Chat? GetChat(string chatId)
        {
            return _chatContext.Chats.Include(x => x.Messages).FirstOrDefault(x => x.Id == chatId);
        }

        public int GetUnreadMessagesCount(string chatId, string userId)
        {
            return _chatContext.Messages
                .Include(x => x.Chat)
                .Where(x => x.Chat.Id == chatId && 
                    x.MessageState == MessageState.SENDED_TO_USER &&
                    x.FromUserId != userId)
                .Count();
        }

        public int GetMessagesCount(string chatId)
        {
            return _chatContext.Messages
                .Include(x => x.Chat)
                .Where(x => x.Chat.Id == chatId && x.MessageState == MessageState.SENDED_TO_USER)
                .Count();
        }

        public Message SendMessage(MessageDTO message)
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
                Id = message.Id ?? Guid.NewGuid().ToString(),
                ContentType = message.ContentType,
                ToUserId = message.UserIdTo,
                MessageState = MessageState.SENDED_TO_SERVER
            };
            chat.Messages.Add(msg);
            _chatContext.SaveChanges();
            return msg;
        }

        public Message SendToUser(MessageDTO message)
        {
            var chat = _chatContext.Chats.First(x => x.Id == message.ChatId);
            var msg = _chatContext.Messages.FirstOrDefault(x => x.Id == message.Id);
            msg.MessageState = MessageState.SENDED_TO_USER;
            _chatContext.SaveChanges();
            return msg;
        }

        public void ReadMessages(IEnumerable<string> messages, string chatId)
        {
            var chat = _chatContext.Chats.
                Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == chatId);
            if (chat == null) return;
            var selected = chat.Messages.IntersectBy(messages, x => x.Id);
            foreach (var msg in selected)
                msg.MessageState = MessageState.READ;
            _chatContext.SaveChanges();
        }

        public bool IsChatExisting(string user1, string user2)
        {
            var usersChatsCount = _chatContext.Chats
                .Include(x => x.Users)
                .Select(x => x.Users)
                .Where(x => x.Select(x => x.Id).Contains(user1))
                .Where(x => x.Select(x => x.Id).Contains(user2))
                .Count();

            return usersChatsCount > 0;
        }
    }
}
