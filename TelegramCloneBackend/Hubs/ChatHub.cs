using Microsoft.AspNetCore.SignalR;
using DatabaseLayer.Models.DTO;
using DatabaseLayer.Repositories;
using DatabaseLayer.Repositories.Base;
using DatabaseLayer.Models;

namespace TGBackend.Hubs
{
    public class ChatHub : Hub
    {
        private IConnectionRepository _connectionRepository;
        private IMessagingRepository _messagingRepository;
        private IChatRepository _chatRepository;
        public ChatHub(IMessagingRepository messagingRepository, 
            IConnectionRepository connectionRepository , 
            IChatRepository chatRepository)
        {
            _connectionRepository = connectionRepository;
            _messagingRepository = messagingRepository;
            _chatRepository = chatRepository;
        }

        public async Task ReadMessage(IEnumerable<string> messageIds, string chatId, string targetUserId)
        {
            var connections = _connectionRepository.GetUserConnections(targetUserId);
            _messagingRepository.ReadMessages(messageIds, chatId);
            await Clients.Clients(connections.Select(x => x.ConnectionID)).SendAsync("ReadMessage", messageIds, chatId, targetUserId);
            await Clients.Caller.SendAsync("ReadMessage", messageIds, chatId, targetUserId);
        }

        public async Task SendMessage(MessageDTO data)
        {
            var userToConnections = _connectionRepository.GetUserConnections(data.UserIdTo);
            var sendedMsg = _messagingRepository.SendMessage(data, out ChatView reciever);
            data.Created = sendedMsg.Created;
            if(reciever != null)
                await Clients.Clients(userToConnections.Select(x => x.ConnectionID)).SendAsync("ReceiveMessage", data, reciever);
            else await Clients.Clients(userToConnections.Select(x => x.ConnectionID))
                .SendAsync("ReceiveMessage", data);
            var msg = _messagingRepository.SendToUser(data);
            data.State = msg.MessageState;
            await Clients.Caller.SendAsync("ReceiveMessage", data, reciever);
        }
        public void SetUserHub(string userId)
        {
            _connectionRepository.OnConnect(userId, Context.ConnectionId,
                Context.GetHttpContext().Request.Headers["User-Agent"]);
        }

        public async Task SendToMe(MessageDTO message)
        {
            var sendedMsg = _messagingRepository.SendMessage(message, out ChatView reciever);
            var msg = _messagingRepository.SendToUser(message);
            message.Created = msg.Created;
            message.State = msg.MessageState;
            await Clients.Caller.SendAsync("SendToMe", message);
        }
    }

}
