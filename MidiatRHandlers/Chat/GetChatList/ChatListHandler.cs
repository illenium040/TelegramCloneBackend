using Database.Models;
using Database.Models.DTO;
using Database.Repositories;
using MediatR;

namespace MidiatRHandlers.Chat.GetChatList
{
    internal class ChatListHandler : IRequestHandler<ChatListQuery, RequestResult<IEnumerable<ChatListUnit>>>
    {
        private readonly ChatRepository _chatRepository;
        private readonly UserRepository _userRepository;

        public ChatListHandler(ChatRepository chatRepository, UserRepository repository)
        {
            _chatRepository = chatRepository;
            _userRepository = repository;
        }
        public async Task<RequestResult<IEnumerable<ChatListUnit>>> Handle(ChatListQuery request, CancellationToken cancellationToken)
        {
            var chatList = _userRepository.GetUserChatList(request.UserId);
            var units = GetUnits(chatList, request.UserId).ToList();
            return new RequestResult<IEnumerable<ChatListUnit>>
            {
                Data = units,
                Status = System.Net.HttpStatusCode.OK,
                Succeeded = true
            };
        }

        private IEnumerable<ChatListUnit> GetUnits(IEnumerable<Database.Models.Chat> chatList, string userId)
        {
            foreach (var chat in chatList)
            {
                var lm = _chatRepository.GetLastMessageFromChat(chat.Id);
                var msc = _chatRepository.GetUnreadMessagesCount(chat.Id, userId);
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
                        Name = user.DisplayName
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
    }
}
