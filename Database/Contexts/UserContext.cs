using Microsoft.EntityFrameworkCore;
using Database.Models;
using DatabaseLayer.Models;

namespace Database.Contexts
{
    public class UserContext : IdentityDbBase
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<ChatToUser> ChatsToUsers { get; set; }
    }
}
