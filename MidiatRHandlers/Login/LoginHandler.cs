using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Rest;
using System.Net;
using Database.Models;
using MediatR.JWT;
using MediatR.Handlers.Models;

namespace MediatR.Handlers.Login
{
    public class LoginHandler : IRequestHandler<LoginQuery, UserModel>
    {
		private readonly UserManager<User> _userManager;
		private readonly IJwtGenerator _jwtGenerator;
		private readonly SignInManager<User> _signInManager;

		public LoginHandler(UserManager<User> userManager, SignInManager<User> signInManager, IJwtGenerator gen)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_jwtGenerator = gen;
		}

		public async Task<UserModel> Handle(LoginQuery request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null)
			{
				throw new RestException(HttpStatusCode.Unauthorized.ToString());
			}

			var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

			if (result.Succeeded)
			{
				return new UserModel
				{
					DisplayName = user.DisplayName,
					Token = _jwtGenerator.CreateToken(user),                         
					UserName = user.UserName
				};
			}

			throw new RestException(HttpStatusCode.Unauthorized.ToString());
		}
	}
}
