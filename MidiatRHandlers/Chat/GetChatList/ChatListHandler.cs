using DatabaseLayer.Models;
using DatabaseLayer.Models.DTO;
using DatabaseLayer.Repositories;
using DatabaseLayer.Models;
using DatabaseLayer.Repositories;
using MediatR;
using DatabaseLayer.Repositories.Base;
using DatabaseLayer.Models.Extensions;

namespace MidiatRHandlers.Chat.GetChatList
{
    internal class ChatListHandler : IRequestHandler<ChatListQuery, RequestResult<IEnumerable<ChatView>>>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserChatRepository _userChatRepository;
        public ChatListHandler(IChatRepository chatRepository, 
            IUserRepository repository, 
            IUserChatRepository userChatRepository)
        {
            _chatRepository = chatRepository;
            _userRepository = repository;
            _userChatRepository = userChatRepository;
        }
        public async Task<RequestResult<IEnumerable<ChatView>>> Handle(ChatListQuery request, CancellationToken cancellationToken)
        {
            var chatList = _userChatRepository.GetUserChatList(request.UserId).ToList();
            if (!chatList.Any()) return new RequestResult<IEnumerable<ChatView>>
            {
                Data = Enumerable.Empty<ChatView>(),
                Status = System.Net.HttpStatusCode.OK,
                Succeeded = true
            };
            var units = GetUnits(chatList).ToList();
            return new RequestResult<IEnumerable<ChatView>>
            {
                Data = units,
                Status = System.Net.HttpStatusCode.OK,
                Succeeded = true
            };
        }

        private IEnumerable<ChatView> GetUnits(IEnumerable<ChatToUser> chatList)
        {
            foreach (var chat in chatList)
            {
                var lm = _chatRepository.GetLastMessageFromChat(chat.ChatId);
                var msc = _chatRepository.GetUnreadMessagesCount(chat.ChatId, chat.UserId);
                var userChat = _userRepository.Get(chat.TargetUserId);
                var chatListUnit = new ChatView
                {
                    ChatId = chat.ChatId,
                    UnreadMessagesCount = msc,
                    User = userChat.ToDTO(),
                    ChatToUser = chat.ToDTO(),
                    LastMessage = lm is null ? null : lm.ToDTO()
                };
                yield return chatListUnit;
            }
        }
    }
}
