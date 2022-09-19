using Microsoft.EntityFrameworkCore;
using Database.Models;
using DatabaseLayer.Models;

namespace Database.Contexts
{
    public class ChatContext : DbContextBase
    {
        public ChatContext(DbContextOptions<ChatContext> options) : base(options) { }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatToUser> ChatsToUsers { get; set; }
    }
}
