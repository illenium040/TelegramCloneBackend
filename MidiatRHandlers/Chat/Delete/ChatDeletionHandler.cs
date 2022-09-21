using DatabaseLayer.Repositories;
using DatabaseLayer.Repositories.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiatRHandlers.Chat.Delete
{
    internal class ChatDeletionHandler : IRequestHandler<ChatDeletionQuery, RequestResult>
    {
        private readonly IUserChatRepository _userChatRepository;
        public ChatDeletionHandler(IUserChatRepository userChatRepository)
        {
            _userChatRepository = userChatRepository;
        }

        public Task<RequestResult> Handle(ChatDeletionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _userChatRepository.RemoveChat(request.ChatId, request.UserId);
                return Task.FromResult(RequestResult.OK());
            }
            catch(Exception e)
            {
                return Task.FromResult(RequestResult.BadRequest(new List<string> { e.Message }));
            }
        }
    }
}
