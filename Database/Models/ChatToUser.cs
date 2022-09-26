using DatabaseLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Models
{
    [Table("ChatsToUsers")]
    public class ChatToUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string ChatId { get; set; }
        public Chat Chat { get; set; }
        public string TargetUserId { get; set; }
        public bool IsPrivate { get; set; } = true;
        public bool IsArchived { get; set; } = false;
        public bool IsNotified { get; set; } = true;
        public bool IsPinned { get; set; } = false;
        public bool IsBlocked { get; set; } = false;
        public ICollection<Folder> Folders { get; set; } = new List<Folder>();
    }
}
