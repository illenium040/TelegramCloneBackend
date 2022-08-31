namespace TelegramCloneBackend.Database.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Avatar { get; set; }
        public string Email { get; set; }
        public List<Chat> Chats { get; set; }
        public List<string> HubConnections { get; set; }
        = new List<string>();


    }
}
