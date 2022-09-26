using DatabaseLayer.Contexts;
using DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;
using DatabaseLayer.Repositories.Base;
using DatabaseLayer.Models.DTO;
using DatabaseLayer.Models;

namespace DatabaseLayer.Repositories
{
    public class PrivateChatRepository : IChatRepository
    {
        private ChatContext _chatContext;
        public PrivateChatRepository(ChatContext context)
        {
            _chatContext = context;
        }

        public Message? GetLastMessageFromChat(string chatId)
        {
            var chat = _chatContext.Chats
                .Include(x => x.Messages)
                .Where(x => x.Id == chatId);
            var messages = chat.SelectMany(x => x.Messages);
            if (messages.Count() == 0) return null;
            return messages.OrderBy(x => x.Created)?.Last();
        }

        public IEnumerable<Message> GetMessages(string chatId)
        {
            return _chatContext.Chats.Include(x => x.Messages)
                .Where(x => x.Id == chatId)
                .SelectMany(x => x.Messages);
        }

        public int GetUnreadMessagesCount(string chatId, string userId)
        {
            return _chatContext.Messages
                .Include(x => x.Chat)
                .Where(x => x.Chat.Id == chatId && 
                    x.MessageState == MessageState.SENDED_TO_USER &&
                    x.Sender != userId)
                .Count();
        }

        public int GetMessagesCount(string chatId)
        {
            return _chatContext.Messages
                .Include(x => x.Chat)
                .Where(x => x.Chat.Id == chatId && x.MessageState == MessageState.SENDED_TO_USER)
                .Count();
        }

        public void Add(Chat entity)
        {
            if(_chatContext.Chats.Find(entity.Id) == null)
                _chatContext.Chats.Add(entity);
            _chatContext.SaveChanges();
        }

        public Chat Get(string id) => _chatContext.Chats.Find(id);
        public IEnumerable<Chat> GetAll() => _chatContext.Chats.AsNoTracking();
        public void Save() => _chatContext.SaveChanges();
    }
}
