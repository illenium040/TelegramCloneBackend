using Microsoft.AspNetCore.Identity;
using MediatR.Database.Contexts;
using MediatR.Database.Models;

namespace MediatR
{
    public class DataSeed
    {
        public static async Task SeedDataAsync(UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<User>
                            {
                                new User
                                    {
                                        DisplayName = "TestUserFirst",
                                        UserName = "TestUserFirst",
                                        Email = "test@test.com"
                                    },

                                new User
                                    {
                                        DisplayName = "TestUserSecond",
                                        UserName = "TestUserSecond",
                                        Email = "testusersecond@test.com"
                                    }
                              };
                try
                {
                    foreach (var user in users)
                    {
                        await userManager.CreateAsync(user, "qazwsX123@");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
