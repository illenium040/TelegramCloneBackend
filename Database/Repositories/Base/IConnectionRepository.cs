namespace Database.Repositories.Base
{
    public interface IConnectionRepository
    {
        void OnConnect(string userId, string connectionId, string userAgent);
        void OnDisconnect(string userId, string userAgent);
    }
}
