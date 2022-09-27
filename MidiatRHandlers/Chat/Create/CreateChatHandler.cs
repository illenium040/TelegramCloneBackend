using DatabaseLayer.Repositories;
using DatabaseLayer.Repositories.Base;
using MediatR;

namespace CQRSLayer.Chat.Create
{
    public class CreateChatHandler : ICommandHandler<CreateChatCommand, string>
    {
        private readonly IUserChatRepository _repo;
        public CreateChatHandler(IUserChatRepository userChatRepo) { _repo = userChatRepo; }

        public Task<CommandResult<string>> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            var res = _repo.AddChat(request.UserId, request.WithUserId);
            if (res == null) return Task.FromResult(new CommandResult<string>
            {
                Result = CommandResult.BadRequest()
            });
            return Task.FromResult(new CommandResult<string>
            {
                Data = res,
                Result = CommandResult.OK()
            });
        }
    }
}
