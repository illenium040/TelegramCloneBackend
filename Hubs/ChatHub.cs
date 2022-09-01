using Microsoft.AspNetCore.SignalR;
using TelegramCloneBackend.Database.Models.DTO;
using TelegramCloneBackend.Database.Repositories;

namespace TelegramCloneBackend.Hubs
{
    public class ChatHub : Hub
    {
        private ChatRepository _chatRepository;
        private UserRepository _userRepository;
        public ChatHub(ChatRepository chatRepository, UserRepository userRepository)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
        }
        public async Task SendMessage(MessageToServerDTO data)
        {
            var userToConnections = _userRepository.GetUserConnections(data.UserIdTo);
            var sendedMsg = _chatRepository.SendMessage(data);
            var clientMessage = new MessageDTO
            {
                Content = sendedMsg.Content,
                Created = sendedMsg.Created,
                UserIdFrom = sendedMsg.FromUserId,
                Id = sendedMsg.Id
            };
            await Clients.Caller.SendAsync("MessageSended", clientMessage);
            await Clients.Clients(userToConnections.Select(x => x.ConnectionID))
                .SendAsync("ReceiveMessage",clientMessage);
        }
        public void SetUserHub(string userId)
        {
            _userRepository.OnConnect(userId, Context.ConnectionId,
                Context.GetHttpContext().Request.Headers["User-Agent"]);
        }
    }

}
