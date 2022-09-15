using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("Users")]
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }
        public string? Avatar { get; set; }
        public ICollection<Chat>? Chats { get; set; }
        public ICollection<Connection>? Connections { get; set; }

    }
}
