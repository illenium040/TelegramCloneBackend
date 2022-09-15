using Database.Models;

namespace MediatR.JWT
{
    public interface IJwtGenerator
    {
        string CreateToken(User user);
    }
}
