using Database.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiatRHandlers.Chat.Delete
{
    internal class ChatDeletionHandler : IRequestHandler<ChatDeletionQuery, RequestResult<string>>
    {
        private readonly PrivateChatRepository _chatRepository;
        private readonly UserRepository _userRepository;
        public ChatDeletionHandler(PrivateChatRepository privateChat, UserRepository userRepository)
        {
            _userRepository = userRepository;
            _chatRepository = privateChat;
        }

        public Task<RequestResult<string>> Handle(ChatDeletionQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
