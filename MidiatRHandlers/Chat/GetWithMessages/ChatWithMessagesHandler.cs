using Database.Models.DTO;
using Database.Repositories;
using MediatR;

namespace MidiatRHandlers.Chat.GetWithMessages
{
    public class ChatWithMessagesHandler : IRequestHandler<ChatWithMessagesQuery, RequestResult<ChatDTO>>
    {
        private readonly PrivateChatRepository _chatRepository;
        public ChatWithMessagesHandler(PrivateChatRepository chat)
        {
            _chatRepository = chat;
        }
        public async Task<RequestResult<ChatDTO>> Handle(ChatWithMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = _chatRepository.GetMessages(request.ChatId);
            if (messages == null) return new RequestResult<ChatDTO>
            {
                Errors = new List<string>() { "Chat not found" },
                Status = System.Net.HttpStatusCode.BadRequest
            };

            return new RequestResult<ChatDTO>
            {
                Status = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Data = new ChatDTO
                {
                    Id = request.ChatId,
                    Messages = messages.OrderBy(x => x.Created)
                    .Select(x => new MessageDTO
                    {
                        Id = x.Id,
                        Content = x.Content,
                        Created = x.Created,
                        UserIdFrom = x.FromUserId,
                        ChatId = request.ChatId,
                        State = x.MessageState
                    }).ToList()
                }
            };
        }
    }
}
