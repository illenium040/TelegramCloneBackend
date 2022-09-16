using MediatR;
using MediatR.Handlers.Models;

namespace MidiatRHandlers.Register
{
    public class RegisterQuery : IRequest<RequestResult>
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
