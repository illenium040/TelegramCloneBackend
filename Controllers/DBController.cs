using Microsoft.AspNetCore.Mvc;
using TelegramCloneBackend.Database.Contexts;
using TelegramCloneBackend.Database.Models;
using TelegramCloneBackend.Database.Models.DTO;
using TelegramCloneBackend.Database.Repositories;

namespace TelegramCloneBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DBController : ControllerBase
    {
        private ChatRepository _chatRepository;
        private UserRepository _userRepository;
        public DBController(ChatRepository context, UserRepository userRepository)
        {
            _chatRepository = context;
            _userRepository = userRepository;
        }

        [HttpGet("chat/{chatid}")]
        public ChatDTO GetChatWithMessages(string chatId)
        {
            var chat = _chatRepository.GetChat(chatId);
            return new ChatDTO
            {
                Id = chat.Id,
                Messages = chat.Messages.OrderBy(x => x.Created)
                .Select(x => new MessageDTO
                {
                    Id = x.Id,
                    Content = x.Content,
                    Created = x.Created,
                    UserIdFrom = x.FromUserId
                }).ToList()
            };
        }

        [HttpGet("chatlist/{userid}")]
        public IEnumerable<ChatListUnit> GetChatList(string userId)
        {
            var chatList = _userRepository.GetUserChatList(userId);
            foreach (var chat in chatList)
            {
                var lm = _chatRepository.GetLastMessageFromChat(chat.Id);
                var msc = _chatRepository.GetMessagesCount(chat.Id);
                var user = chat.Users.First(x => x.Id != userId);
                var chatListUnit = new ChatListUnit
                {
                    ChatId = chat.Id,
                    UnreadMessagesCount = msc,
                    User = new UserDTO
                    {
                        Avatar = user.Avatar,
                        Email = user.Email,
                        Id = user.Id,
                        Name = user.Name
                    }
                };
                if (lm != null)
                    chatListUnit.LastMessage = new MessageDTO
                    {
                        Content = lm.Content,
                        Created = lm.Created,
                        Id = lm.Id,
                        UserIdFrom = lm.FromUserId
                    };
                yield return chatListUnit;
            }
        }


#if DEBUG
        [HttpGet("user/{name}")]
        public async Task<UserDTO> GetUser(string name)
        {
            var user = _userRepository.GetByName(name);
            return new UserDTO
            {
                Id = user.Id,
                Avatar = user.Avatar,
                Email = user.Email,
                Name = user.Name
            };
        }

        [HttpGet("createchat")]
        public async Task CreateChatWithUser()
        {
            var id = Guid.NewGuid().ToString();
            _userRepository.Add(new User
            {
                Id = id,
                Avatar = "images/gigachad.jpg",
                Email = "gigachad@chad.mail.gg",
                Name = "Олег"
            });
            var user2 = _userRepository.GetByName("Виталий");
            _userRepository.CreateChatBetweenUsers(id, user2.Id);
        }

        //used only once
        [HttpGet("init")]
        public async Task InitDBG()
        {
            //Add users
            var firstUserId = Guid.NewGuid().ToString();
            var secondUSerId = Guid.NewGuid().ToString();
            _userRepository.Add(new User
            {
                Id = firstUserId,
                Avatar = "images/gigachad.jpg",
                Email = "gigachad@chad.mail.gg",
                Name = "Виталий"
            });
            _userRepository.Add(new User
            {
                Id = secondUSerId,
                Avatar = "images/davida.jpg",
                Name = "Давыда",
                Email = "zhizha@mail.ru"
            });

            var chatId = _userRepository.CreateChatBetweenUsers(firstUserId, secondUSerId);
            _chatRepository.SendMessage(new MessageDTO
            {
                ChatId = chatId,
                Content = "Здарова, братан",
                UserIdFrom = firstUserId,
                UserIdTo = secondUSerId
            });
            _chatRepository.SendMessage(new MessageDTO
            {
                ChatId = chatId,
                Content = "По пиуку?",
                UserIdFrom = firstUserId,
                UserIdTo = secondUSerId
            });
            _chatRepository.SendMessage(new MessageDTO
            {
                ChatId = chatId,
                Content = "Здаровa",
                UserIdFrom = secondUSerId,
                UserIdTo = firstUserId
            });
            _chatRepository.SendMessage(new MessageDTO
            {
                ChatId = chatId,
                Content = "Во сколько?",
                UserIdFrom = secondUSerId,
                UserIdTo = firstUserId
            });
        }
#endif

    }
}
