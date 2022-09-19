using Microsoft.EntityFrameworkCore;
using Database.Contexts;
using Database.Models;
using Database.Repositories.Base;
using DatabaseLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Database.Repositories
{
    public class UserRepository : IUserRepository, IConnectionRepository
    {
        private UserContext _userContext;
        private UserManager<User> _manager;
        public UserRepository(UserContext userContext, UserManager<User> manager)
        {
            _userContext = userContext;
            _manager = manager;
        }

        public void Add(User user)
        {
            _userContext.Users.Add(user);
            _userContext.SaveChanges();
        }

        public IEnumerable<User> Seacrh(string name)
        {
            if (name[0] == '@')
                return _userContext.Users
                        .Where(x => EF.Functions.Like(x.UserName, $"%{name.Substring(1)}%"))
                        .AsEnumerable();
            return _userContext.Users
                .Where(x => EF.Functions.Like(x.DisplayName, $"%{name}%"))
                .AsEnumerable();
        }

        public User Get(string id)
        {
            return _userContext.Users
                .SingleOrDefault(x => x.Id == id);
        }

        public User GetByName(string name)
        {
            return _userContext.Users.SingleOrDefault(x => x.DisplayName == name);
        }

        
        public IEnumerable<Connection> GetUserConnections(string id)
        {
            var user = _userContext.Users
                .Include(x => x.Connections)
                .SingleOrDefault(x => x.Id == id);
            return user?.Connections ?? Enumerable.Empty<Connection>();
        }

        public IEnumerable<User> GetUsers()
        {
            return _userContext.Users.OrderBy(x => x.DisplayName).ToList();
        }

        public void OnConnect(string userId, string connectionId, string userAgent)
        {
            var user = _userContext.Users
                .Include(x => x.Connections)
                .SingleOrDefault(x => x.Id == userId);

            if (user == null) return;

            if (user.Connections == null)
                user.Connections = new List<Connection>();

            var connection = user.Connections
                .Where(c => c.UserAgent == userAgent)
                .FirstOrDefault();

            if (connection == null)
            {
                connection = new Connection();
                connection.UserAgent = userAgent;
                user.Connections.Add(connection);
            }

            connection.ConnectionID = connectionId;
            connection.Connected = true;

            _userContext.SaveChanges();
        }

        public void OnDisconnect(string userId, string userAgent)
        {
            throw new NotImplementedException();
        }
    }
}
