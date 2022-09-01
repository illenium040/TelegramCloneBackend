using Microsoft.EntityFrameworkCore;
using TelegramCloneBackend.Database.Models;

namespace TelegramCloneBackend.Database.Contexts
{
    public class DbContextBase : DbContext
    {
        public DbContextBase(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Set up keys
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Chat>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Message>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Connection>()
                .HasKey(x => x.Id);
            //Set up relations
            modelBuilder.Entity<Chat>()
                .HasMany(x => x.Users)
                .WithMany(x => x.Chats);
            modelBuilder.Entity<User>()
                .HasMany(x => x.Chats)
                .WithMany(x => x.Users);
            modelBuilder.Entity<Message>()
                .HasOne(x => x.Chat)
                .WithMany(x => x.Messages);
            modelBuilder.Entity<Connection>()
                .HasOne(x => x.User)
                .WithMany(x => x.Connections);

            modelBuilder.Entity<Connection>()
                .Property(x => x.Id)
                .UseSerialColumn();
        }
    }
}
