using System.Collections;
using DatabaseLayer.Models;
using DatabaseLayer.Repositories.Base;

namespace DatabaseLayer.Repositories.Base
{
    public interface IUserRepository : IRepository<User>
    {
        User? GetByName(string name);
        IEnumerable<User> Seacrh(string name);
        void AddFolder(Folder folder);
    }
}
