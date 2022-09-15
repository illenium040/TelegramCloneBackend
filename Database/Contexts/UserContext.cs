using Microsoft.EntityFrameworkCore;
using Database.Models;

namespace Database.Contexts
{
    public class UserContext : IdentityDbBase
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Connection> Connections { get; set; }
    }
}
