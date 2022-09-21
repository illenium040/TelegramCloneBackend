﻿using DatabaseLayer.Contexts;
using DatabaseLayer.Models;
using DatabaseLayer.Models.DTO;
using DatabaseLayer.Models;
using DatabaseLayer.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer.Models.Extensions;

namespace DatabaseLayer.Repositories
{
    public class UserChatRepository : IUserChatRepository, IMessagingRepository
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

        private void AddUserToChat(string chatId, string userId)
        {
            var user = _userContext.Users.Include(x => x.Chats).SingleOrDefault(x => x.Id == userId);
            var chat = _chatContext.Chats.Include(x => x.Users).SingleOrDefault(x => x.Id == chatId);
            var targetId = chat.Users.First().UserId;
            user.Chats.Add(new ChatToUser
            {
                ChatId = chat.Id,
                UserId = userId,
                TargetUserId = targetId,
            });
            _userContext.SaveChanges();
        }

        public string? AddChat(string userId, string withUserId)
        {
            var userChatWith = _userContext.ChatsToUsers
                .Where(x => x.UserId == userId)
                .FirstOrDefault(x => x.TargetUserId == withUserId);
            if(userChatWith != null) return userChatWith.ChatId;

            var userWithChat = _userContext.ChatsToUsers
                .Where(x => x.UserId == withUserId)
                .FirstOrDefault(x => x.TargetUserId == userId);
            if(userWithChat != null) return userWithChat.ChatId;

            var user = _userContext.Users.Include(x => x.Chats).SingleOrDefault(x => x.Id == userId);
            var targetUser = _userContext.Users.Include(x => x.Chats).SingleOrDefault(x => x.Id == withUserId);

            if (user == null || targetUser == null) return null;

            var chat = new Chat { Id = Guid.NewGuid().ToString() };
            _chatContext.Add(chat);
            _chatContext.SaveChanges();
            user.Chats.Add(new ChatToUser { ChatId = chat.Id, UserId = userId, TargetUserId = withUserId });
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

        public Message SendMessage(MessageDTO message, out ChatView receiver)
        {
            receiver = null;
            User targetUser = null;
            var chat = _chatContext.Chats
                .Include(x => x.Users)
                .Include(x => x.Messages)
                .SingleOrDefault(x => x.Id == message.ChatId);
            //if (chat == null) throw new Exception("Chat isn't existing");

            var chatUsers = chat.Users.Select(x => x.UserId).ToList();
            if (!chatUsers.Contains(message.UserIdFrom))
            {
                AddUserToChat(message.ChatId, message.UserIdFrom);
                targetUser = _manager.Users.SingleOrDefault(x => x.Id == message.UserIdTo);
            }
            if (!chatUsers.Contains(message.UserIdTo))
            {
                AddUserToChat(message.ChatId, message.UserIdTo);
                targetUser = _manager.Users.SingleOrDefault(x => x.Id == message.UserIdFrom);
            }

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

            if (targetUser != null)
            {
                receiver = new ChatView
                {
                    ChatId = message.ChatId,
                    LastMessage = msg.ToDTO(),
                    User = targetUser.ToDTO(),
                };
            }

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

        public void RemoveChat(string chatId, string userId)
        {
            var chat = _chatContext.Chats.Include(x => x.Users).SingleOrDefault(x => x.Id == chatId);
            if (chat != null && chat.Users.Count == 1)
            {
                _chatContext.Remove(chat);
                _chatContext.SaveChanges();
                return;
            }
            var ctu = _userContext.ChatsToUsers.SingleOrDefault(x => x.UserId == userId && x.ChatId == chatId);
            if (ctu == null) throw new Exception("Chat isn't existing");
            _userContext.ChatsToUsers.Remove(ctu);
            _userContext.SaveChanges();
            
        }
    }
}
