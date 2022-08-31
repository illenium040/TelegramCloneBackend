﻿namespace TelegramCloneBackend.Database.Models
{
    public class Message
    {
        public Chat Chat { get; set; }
        public string Id { get; set; }
        public string FromUserId { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
    }
}
