using Database.Repositories;
using MediatR;

namespace MidiatRHandlers.Chat.CreateChat
{
    public class ChatCreationHandler : IRequestHandler<ChatCreationQuery, RequestResult<string>>
    {
        private readonly ChatRepository _chatRepository;
        private readonly UserRepository _userRepository;
        public ChatCreationHandler(ChatRepository chatRepository, UserRepository repository)
        {
            _chatRepository = chatRepository;
            _userRepository = repository;
        }

        public Task<RequestResult<string>> Handle(ChatCreationQuery request, CancellationToken cancellationToken)
        {
            var res = _chatRepository.IsChatExisting(request.CurrentUser, request.TargetUser);
            if (res) return Task.FromResult(new RequestResult<string>
            {
                Errors = new List<string> { "Chat is already existing" },
                Status = System.Net.HttpStatusCode.BadRequest
            });
            return Task.FromResult(new RequestResult<string>
            {
                Data = _userRepository.CreateChatBetweenUsers(request.CurrentUser, request.TargetUser),
                Status = System.Net.HttpStatusCode.OK,
                Succeeded = true
            });

        }
    }
}
