using MediatR;
using MediatR.Handlers.Models;

namespace MediatR.Handlers.Login
{
	public class LoginQuery : IRequest<UserModel>
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
