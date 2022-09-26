using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseLayer.Models
{
    [Table("Chats")]
    public class Chat
    {
        public string Id { get; set; }
        public ICollection<Message>? Messages { get; set; } = new List<Message>();
        public ICollection<ChatToUser> Users { get; set; }
    }
}
