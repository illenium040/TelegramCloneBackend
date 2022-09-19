using Database.Contexts;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Database.Repositories.Base;
using Database.Models.DTO;
using DatabaseLayer.Models;

namespace Database.Repositories
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
    }
}
