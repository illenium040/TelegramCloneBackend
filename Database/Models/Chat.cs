using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("Chats")]
    public class Chat
    {
        public string Id { get; set; }
        public List<Message>? Messages { get; set; } = new List<Message>();
        public List<User> Users { get; set; } = new List<User>();

    }
}
