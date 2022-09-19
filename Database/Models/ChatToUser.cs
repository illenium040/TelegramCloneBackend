using Database.Models;
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
    }
}
