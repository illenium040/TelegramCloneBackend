using Microsoft.EntityFrameworkCore;
using DatabaseLayer.Models;
using DatabaseLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace DatabaseLayer.Contexts
{
    public class DbContextBase : DbContext
    {
        public DbContextBase(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Chat).WithMany(x => x.Messages).HasForeignKey(x => x.ChatId);
            });
            modelBuilder.Entity<Chat>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasMany(x => x.Users).WithOne(x => x.Chat).HasForeignKey(x => x.ChatId);
                e.HasMany(x => x.Messages).WithOne(x => x.Chat).HasForeignKey(x => x.ChatId);
            });

            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasMany(x => x.Chats).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            });

            modelBuilder.Entity<ChatToUser>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).UseSerialColumn().ValueGeneratedOnAdd();
                e.HasOne(x => x.User).WithMany(x => x.Chats).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Chat).WithMany(x => x.Users).HasForeignKey(x => x.ChatId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Connection>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.User).WithMany(x => x.Connections);
                e.Property(x => x.Id).UseSerialColumn();
            });
        }

        
    }
}
