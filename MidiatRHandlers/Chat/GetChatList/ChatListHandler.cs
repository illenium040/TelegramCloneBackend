using Database.Models;
using Database.Models.DTO;
using Database.Repositories;
using DatabaseLayer.Models;
using DatabaseLayer.Repositories;
using MediatR;

namespace MidiatRHandlers.Chat.GetChatList
{
    internal class ChatListHandler : IRequestHandler<ChatListQuery, RequestResult<IEnumerable<ChatListUnit>>>
    {
        private readonly PrivateChatRepository _chatRepository;
        private readonly UserChatRepository _userRepository;

        public ChatListHandler(PrivateChatRepository chatRepository, UserChatRepository repository)
        {
            _chatRepository = chatRepository;
            _userRepository = repository;
        }
        public async Task<RequestResult<IEnumerable<ChatListUnit>>> Handle(ChatListQuery request, CancellationToken cancellationToken)
        {
            var chatList = _userRepository.GetUserChatList(request.UserId);
            if (!chatList.Any()) return new RequestResult<IEnumerable<ChatListUnit>>
            {
                Data = Enumerable.Empty<ChatListUnit>(),
                Status = System.Net.HttpStatusCode.OK,
                Succeeded = true
            };
            var units = GetUnits(chatList, request.UserId).ToList();
            return new RequestResult<IEnumerable<ChatListUnit>>
            {
                Data = units,
                Status = System.Net.HttpStatusCode.OK,
                Succeeded = true
            };
        }

        private IEnumerable<ChatListUnit> GetUnits(IEnumerable<DatabaseLayer.Models.Chat> chatList, string userId)
        {
            foreach (var chat in chatList)
            {
                var lm = _chatRepository.GetLastMessageFromChat(chat.Id);
                var msc = _chatRepository.GetUnreadMessagesCount(chat.Id, userId);
                var user = chat.Users.SingleOrDefault(x => x.UserId != userId)?.User;
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
