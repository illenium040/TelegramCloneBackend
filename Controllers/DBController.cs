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
        public DBController(ChatRepository context)
        {
            _chatRepository = context;
        }

        [HttpGet("chat/{chatid}")]
        public ChatDTO GetChatWithMessages(string chatId)
        {
            var chat = _chatRepository.GetChat(chatId);
            return new ChatDTO
            {
                Id = chat.Id,
                Messages = chat.Messages.Select(x => new MessageDTO
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
            var chatList = _chatRepository.GetUserChatList(userId);
            foreach (var chat in chatList)
            {
                var lm = _chatRepository.GetLastMessageFromChat(chat.Id);
                var msc = _chatRepository.GetMessagesCount(chat.Id);
                var user = chat.Users.First(x => x.Id != userId);
                yield return new ChatListUnit
                {
                    ChatId = chat.Id,
                    LastMessage = new MessageDTO() {
                        Content = lm.Content,
                        Created = lm.Created,
                        UserIdFrom = lm.FromUserId,
                        Id = lm.Id
                    },
                    UnreadMessagesCount = msc,
                    User = new UserDTO
                    {
                        Avatar = user.Avatar,
                        Email = user.Email,
                        Id = user.Id,
                        Name = user.Name
                    }
                };
            }
        }


#if DEBUG
        [HttpGet("user/{index}")]
        public async Task<UserDTO> GetCurrentUser(int index)
        {
            var user = _chatRepository.GetUsers()[index];
            return new UserDTO
            {
                Id = user.Id,
                Avatar = user.Avatar,
                Email = user.Email,
                Name = user.Name
            };
        }

        //used only once
        [HttpGet("init")]
        public async Task InitDBG()
        {
            //Add users
            var firstUserId = Guid.NewGuid().ToString();
            var secondUSerId = Guid.NewGuid().ToString();
            _chatRepository.AddUser(new User
            {
                Id = firstUserId,
                Avatar = "images/gigachad.jpg",
                Email = "gigachad@chad.mail.gg",
                Name = "Виталий"
            });
            _chatRepository.AddUser(new User
            {
                Id = secondUSerId,
                Avatar = "images/davida.jpg",
                Name = "Давыда",
                Email = "zhizha@mail.ru"
            });

            var chatId = _chatRepository.CreateChatBetweenUsers(firstUserId, secondUSerId);
            _chatRepository.SendMessage(firstUserId, chatId, "Здарова, братан");
            _chatRepository.SendMessage(firstUserId, chatId, "По пиуку?");
            _chatRepository.SendMessage(secondUSerId, chatId, "Здаровa");
            _chatRepository.SendMessage(secondUSerId, chatId, "Во сколько?");
        }
#endif

    }
}
