using DatabaseLayer.Models;
using DatabaseLayer.Repositories.Base;
using Microsoft.AspNetCore.Identity;

namespace TelegramCloneBackend
{
    public static class DbSeed
    {
        public static void SeedUsers(UserManager<User> manager, IUserChatRepository repo)
        {
            if (manager.Users.Any()) return;
            User d = new User()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "FirstUser",
                DisplayName = "Давыда",
                Email = "fu@gmail.com",
                LockoutEnabled = false,
                Avatar = "/images/2.jpg"
            };
            User v = new User()
            {
                DisplayName = "Виталий",
                Id = Guid.NewGuid().ToString(),
                UserName = "SecondUser",
                Email = "fs@gmail.com",
                LockoutEnabled = false
            };
            User o = new User()
            {
                DisplayName = "Олег",
                Id = Guid.NewGuid().ToString(),
                UserName = "OlegXD",
                Email = "oleg@gmail.com",
                LockoutEnabled = false
            };

            manager.CreateAsync(d, "aA1234!").Wait();
            manager.CreateAsync(v, "aA1234!").Wait();
            manager.CreateAsync(o, "aA1234!").Wait();

            repo.AddChat(d.Id, d.Id);
            repo.AddChat(v.Id, v.Id);
            repo.AddChat(o.Id, o.Id);
        }
    }
}
