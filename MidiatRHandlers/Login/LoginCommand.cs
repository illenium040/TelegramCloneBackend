using MediatR;
using MediatR.Handlers.Models;
using CQRSLayer;

namespace MediatR.Handlers.Login
{
	public record LoginCommand(string Email, string Password) : ICommand<UserModel>;
}
