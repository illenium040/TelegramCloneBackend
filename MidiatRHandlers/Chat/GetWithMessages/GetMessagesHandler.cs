using DatabaseLayer.Models.DTO;
using DatabaseLayer.Models.Extensions;
using DatabaseLayer.Repositories;
using DatabaseLayer.Repositories.Base;
using MediatR;

namespace CQRSLayer.Chat.GetWithMessages
{
    public class GetMessagesHAndler : ICommandHandler<GetMessagesCommand, ChatDTO>
    {
        private readonly IChatRepository _chatRepository;
        public GetMessagesHAndler(IChatRepository chat)
        {
            _chatRepository = chat;
        }
        public async Task<CommandResult<ChatDTO>> Handle(GetMessagesCommand request, CancellationToken cancellationToken)
        {
            var messages = _chatRepository.GetMessages(request.ChatId);
            if (messages == null) return new CommandResult<ChatDTO>
            {
                Result = CommandResult.BadRequest(new List<string>() { "Chat not found" })
            };

            return new CommandResult<ChatDTO>
            {
                Result = CommandResult.OK(),
                Data = new ChatDTO
                {
                    Id = request.ChatId,
                    Messages = messages.OrderBy(x => x.Created).Select(x => x.ToDTO()).ToList()
                }
            };
        }
    }
}
