using DatabaseLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace TelegramCloneBackend
{
    public static class DbSeed
    {
        public static void SeedUsers(UserManager<User> manager)
        {
            if (manager.Users.Any()) return;
            User fu = new User()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "FirstUser",
                DisplayName = "Давыда",
                Email = "fu@gmail.com",
                LockoutEnabled = false
            };
            User fs = new User()
            {
                DisplayName = "Виталий",
                Id = Guid.NewGuid().ToString(),
                UserName = "SecondUser",
                Email = "fs@gmail.com",
                LockoutEnabled = false
            };

            manager.CreateAsync(fs, "aA1234!").Wait();
            manager.CreateAsync(fu, "aA1234!").Wait();

        }
    }
}
