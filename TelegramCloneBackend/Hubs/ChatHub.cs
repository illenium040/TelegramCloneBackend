using Microsoft.AspNetCore.SignalR;
using Database.Models.DTO;
using Database.Repositories;

namespace TGBackend.Hubs
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

        public async Task ReadMessage(IEnumerable<string> messageIds, string chatId, string targetUserId)
        {
            var connections = _userRepository.GetUserConnections(targetUserId);
            _chatRepository.ReadMessages(messageIds, chatId);
            await Clients.Clients(connections.Select(x => x.ConnectionID)).SendAsync("ReadMessage", messageIds, chatId, targetUserId);
            await Clients.Caller.SendAsync("ReadMessage", messageIds, chatId, targetUserId);
        }

        public async Task SendMessage(MessageDTO data)
        {
            var userToConnections = _userRepository.GetUserConnections(data.UserIdTo);
            var sendedMsg = _chatRepository.SendMessage(data);
            data.Created = sendedMsg.Created;
            await Clients.Clients(userToConnections.Select(x => x.ConnectionID)).SendAsync("ReceiveMessage", data);
            _chatRepository.SendToUser(data);
            await Clients.Caller.SendAsync("ReceiveMessage", data);
            
        }
        public void SetUserHub(string userId)
        {
            _userRepository.OnConnect(userId, Context.ConnectionId,
                Context.GetHttpContext().Request.Headers["User-Agent"]);
        }
    }

}
