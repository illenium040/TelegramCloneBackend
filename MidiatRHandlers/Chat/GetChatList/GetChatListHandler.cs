using DatabaseLayer.Models;
using DatabaseLayer.Models.DTO;
using DatabaseLayer.Repositories.Base;
using DatabaseLayer.Models.Extensions;

namespace CQRSLayer.Chat.GetChatList
{
    internal class GetChatListHandler : ICommandHandler<GetChatListCommand, IEnumerable<ChatView>>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserChatRepository _userChatRepository;
        public GetChatListHandler(IChatRepository chatRepository, 
            IUserRepository repository, 
            IUserChatRepository userChatRepository)
        {
            _chatRepository = chatRepository;
            _userRepository = repository;
            _userChatRepository = userChatRepository;
        }
        public async Task<CommandResult<IEnumerable<ChatView>>> Handle(GetChatListCommand request, CancellationToken cancellationToken)
        {
            var chatList = _userChatRepository.GetUserChatList(request.UserId).ToList();
            if (!chatList.Any()) return new CommandResult<IEnumerable<ChatView>>
            {
                Data = Enumerable.Empty<ChatView>(),
                Result = CommandResult.BadRequest(new List<string> { "User has no chats" })
            };
            var units = GetUnits(chatList).ToList();
            return new CommandResult<IEnumerable<ChatView>>
            {
                Data = units,
                Result = CommandResult.OK()
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
