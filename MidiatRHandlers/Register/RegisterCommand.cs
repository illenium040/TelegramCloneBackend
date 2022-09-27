using MediatR;
using MediatR.Handlers.Models;

namespace CQRSLayer.Register
{
    public record RegisterCommand(
        string DisplayName, 
        string Email, 
        string Password) : ICommand;
}
