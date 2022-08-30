using Microsoft.AspNetCore.SignalR;

namespace TelegramCloneBackend
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string message)
        {

            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        public async Task BroadcastChartData(string data, string connectionId) =>
            await Clients.Client(connectionId).SendAsync("BroadCast", data);
        public string GetConnectionId() => Context.ConnectionId;
    }
}
