using Database.Repositories;
using DatabaseLayer.Repositories;
using MediatR;

namespace MidiatRHandlers.Chat.Create
{
    public class ChatCreationHandler : IRequestHandler<ChatCreationQuery, RequestResult<string>>
    {
        private readonly UserChatRepository _repo;
        public ChatCreationHandler(UserChatRepository userChatRepo)
        {
            _repo = userChatRepo;
        }

        public Task<RequestResult<string>> Handle(ChatCreationQuery request, CancellationToken cancellationToken)
        {
            
            var res = _repo.AddChat(request.UserId, request.WithUserId);
            if (res == null) return Task.FromResult(new RequestResult<string>
            {
                Status = System.Net.HttpStatusCode.BadRequest,
            });
            return Task.FromResult(new RequestResult<string>
            {
                Data = res,
                Status = System.Net.HttpStatusCode.OK,
                Succeeded = true
            });
        }
    }
}
