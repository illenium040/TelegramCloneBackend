using System.Collections;
using Database.Models;
using DatabaseLayer.Models;

namespace Database.Repositories.Base
{
    public interface IUserRepository
    {
        void Add(User user);
        User Get(string id);
        User GetByName(string name);
        IEnumerable<User> GetUsers();
        IEnumerable<Connection> GetUserConnections(string id);
        IEnumerable<User> Seacrh(string name);
    }
}
