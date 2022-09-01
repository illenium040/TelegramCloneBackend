using Microsoft.EntityFrameworkCore;
using TelegramCloneBackend.Database.Models;

namespace TelegramCloneBackend.Database.Contexts
{
    public class UserContext : DbContextBase
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Connection> Connections { get; set; }
    }
}
