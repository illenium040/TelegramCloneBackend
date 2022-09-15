using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Database.Models;

namespace Database.Contexts
{
    public class IdentityDbBase : IdentityDbContext<User>
    {
        public IdentityDbBase(DbContextOptions options) : base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
