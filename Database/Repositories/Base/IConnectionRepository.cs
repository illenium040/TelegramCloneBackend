using DatabaseLayer.Models;

namespace DatabaseLayer.Repositories.Base
{
    public interface IConnectionRepository
    {
        IEnumerable<Connection> GetUserConnections(string id);
        void OnConnect(string userId, string connectionId, string userAgent);
        void OnDisconnect(string userId, string userAgent);
    }
}
