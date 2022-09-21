using DatabaseLayer.Models;

namespace MediatR.JWT
{
    public interface IJwtGenerator
    {
        string CreateToken(User user);
    }
}
