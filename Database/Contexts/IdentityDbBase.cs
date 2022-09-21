using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DatabaseLayer.Models;

namespace DatabaseLayer.Contexts
{
    public class IdentityDbBase : IdentityDbContext<User>
    {
        public IdentityDbBase(DbContextOptions options) : base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
