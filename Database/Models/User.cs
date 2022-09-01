using System.ComponentModel.DataAnnotations.Schema;

namespace TelegramCloneBackend.Database.Models
{
    [Table("Users")]
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Avatar { get; set; }
        public string? Email { get; set; }
        public ICollection<Chat>? Chats { get; set; }
        public ICollection<Connection>? Connections { get; set; }


    }
}
