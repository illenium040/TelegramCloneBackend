using MediatR;
using MediatR.Handlers.Models;
using MidiatRHandlers;

namespace MediatR.Handlers.Login
{
	public class LoginQuery : IRequest<RequestResult<UserModel>>
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
