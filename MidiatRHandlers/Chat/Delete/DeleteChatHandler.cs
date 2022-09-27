using DatabaseLayer.Repositories;
using DatabaseLayer.Repositories.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSLayer.Chat.Delete
{
    internal class DeleteChatHandler : ICommandHandler<DeleteChatCommand>
    {
        private readonly IUserChatRepository _userChatRepository;
        public DeleteChatHandler(IUserChatRepository userChatRepository)
        {
            _userChatRepository = userChatRepository;
        }

        public Task<CommandResult> Handle(DeleteChatCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _userChatRepository.RemoveChat(request.ChatId, request.UserId);
                return Task.FromResult(CommandResult.OK());
            }
            catch(Exception e)
            {
                return Task.FromResult(CommandResult.BadRequest(new List<string> { e.Message }));
            }
        }
    }
}
