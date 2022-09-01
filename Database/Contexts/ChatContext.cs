using Microsoft.EntityFrameworkCore;
using TelegramCloneBackend.Database.Models;

namespace TelegramCloneBackend.Database.Contexts
{
    public class ChatContext : DbContextBase
    {
        public ChatContext(DbContextOptions<ChatContext> options) : base(options) { }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
