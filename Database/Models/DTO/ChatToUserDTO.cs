using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Models.DTO
{
    public class ChatToUserDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ChatId { get; set; }
        public string TargetUserId { get; set; }
        public bool? IsPrivate { get; set; } = true;
        public bool? IsArchived { get; set; } = false;
        public bool? IsNotified { get; set; } = true;
        public bool? IsPinned { get; set; } = false;
        public bool? IsBlocked { get; set; } = false;
        public IEnumerable<FolderDTO>? Folders { get; set; }
    }
}
