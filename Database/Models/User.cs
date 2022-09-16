using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("AspNetUsers")]
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }
        public string? Avatar { get; set; }
        public ICollection<Chat>? Chats { get; set; } = new List<Chat>();
        public ICollection<Connection>? Connections { get; set; } = new List<Connection>();

    }
}
