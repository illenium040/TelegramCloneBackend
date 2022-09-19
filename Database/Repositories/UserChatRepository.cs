using Database.Contexts;
using Database.Models;
using Database.Models.DTO;
using DatabaseLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Repositories
{
    public class UserChatRepository
    {
        private UserContext _userContext;
        private ChatContext _chatContext;
        private UserManager<User> _manager;
        public UserChatRepository(UserContext userContext, ChatContext chatContext, UserManager<User> manager)
        {
            _userContext = userContext;
            _manager = manager;
            _chatContext = chatContext;
        }

        public void AddUserToChat(string chatId, string userId)
        {
            var user = _userContext.Users.Include(x => x.Chats).SingleOrDefault(x => x.Id == userId);
            var chat = _chatContext.Chats.SingleOrDefault(x => x.Id == chatId);
            user.Chats.Add(new ChatToUser
            {
                ChatId = chat.Id,
                UserId = userId
            });
            _userContext.SaveChanges();
        }

        public string? AddChat(string userId, string withUserId)
        {
            var user = _userContext.Users.Include(x => x.Chats).SingleOrDefault(x => x.Id == userId);
            var targetUser = _userContext.Users.Include(x => x.Chats).SingleOrDefault(x => x.Id == withUserId);
            if (user == null || targetUser == null) return null;
            var chat = new Chat { Id = Guid.NewGuid().ToString() };
            _chatContext.Chats.Add(chat);
            _chatContext.SaveChanges();
            user.Chats.Add(new ChatToUser { ChatId = chat.Id, UserId = userId });
            _userContext.SaveChanges();
            return chat.Id;
        }

        public IEnumerable<Chat> GetUserChatList(string userId)
        {
            var chats = _userContext.ChatsToUsers
                .Include(x => x.Chat)
                    .ThenInclude(x => x.Users)
                    .ThenInclude(x => x.User)
                .Where(x => x.UserId == userId)
                .Select(x => x.Chat);
            return chats ?? Enumerable.Empty<Chat>();
        }

        public Message SendToUser(MessageDTO message)
        {
            var msg = _chatContext.Messages.SingleOrDefault(x => x.Id == message.Id);
            msg.MessageState = MessageState.SENDED_TO_USER;
            _chatContext.SaveChanges();
            return msg;
        }

        public Message SendMessage(MessageDTO message)
        {
            var chat = _chatContext.Chats
                .Include(x => x.Users)
                .Include(x => x.Messages)
                .SingleOrDefault(x => x.Id == message.ChatId);
            //if (chat == null) throw new Exception("Chat isn't existing");

            var chatUsers = chat.Users.Select(x => x.UserId).ToList();
            if (!chatUsers.Contains(message.UserIdFrom)) AddUserToChat(message.ChatId, message.UserIdFrom);
            if (!chatUsers.Contains(message.UserIdTo)) AddUserToChat(message.ChatId, message.UserIdTo);

            var msg = new Message
            {
                ChatId = message.ChatId,
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
    }
}
