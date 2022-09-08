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

        public async Task ReadMessage(IEnumerable<string> messsageIds, string chatId, string targetUserId)
        {
            var connections = _userRepository.GetUserConnections(targetUserId);
            _chatRepository.ReadMessages(messsageIds, chatId);
            await Clients.Clients(connections.Select(x => x.ConnectionID)).SendAsync("ReadMessages", messsageIds, chatId, targetUserId);
            await Clients.Caller.SendAsync("ReadMessages", messsageIds, chatId, targetUserId);
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
