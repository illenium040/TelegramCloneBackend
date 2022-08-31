using Microsoft.AspNetCore.SignalR;
using TelegramCloneBackend.Database.Models.DTO;
using TelegramCloneBackend.Database.Repositories;

namespace TelegramCloneBackend.Hubs
{
    public class ChatHub : Hub
    {
        private ChatRepository _chatRepository;
        public ChatHub(ChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }
        public async Task SendMessage(MessageToServerDTO data)
        {
            var userToConnections = _chatRepository.GetUser(data.UserIdTo).HubConnections;
            var userFromConnections = _chatRepository.GetUser(data.UserIdFrom).HubConnections;
            if (!userFromConnections.Contains(Context.ConnectionId))
                userFromConnections.Add(Context.ConnectionId);
            var sendedMsg = _chatRepository.SendMessage(data.UserIdFrom, data.ChatId, data.Content);
            var clientMessage = new MessageDTO
            {
                Content = sendedMsg.Content,
                Created = sendedMsg.Created,
                UserIdFrom = sendedMsg.FromUserId,
                Id = sendedMsg.Id
            };
            await Clients.Clients(userFromConnections).SendAsync("MessageSended", clientMessage);
            await Clients.Clients(userToConnections).SendAsync("ReceiveMessage",clientMessage);
        }
        public void SetUserHub(string userId)
        {
            _chatRepository.SetHubConnection(userId, Context.ConnectionId);
        }
        public void RemoveUserHub(string userId)
        {
            _chatRepository.RemoveHubConnection(userId, Context.ConnectionId);
        }
    }

}
