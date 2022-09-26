using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Models
{
    [Table("Folders")]
    public class Folder
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Icon { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<ChatToUser> ChatToUser { get; set; } = new List<ChatToUser>();
    }
}
