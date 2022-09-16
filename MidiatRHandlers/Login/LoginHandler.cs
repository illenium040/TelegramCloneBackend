using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Rest;
using System.Net;
using Database.Models;
using MediatR.JWT;
using MediatR.Handlers.Models;
using MidiatRHandlers;

namespace MediatR.Handlers.Login
{
    public class LoginHandler : IRequestHandler<LoginQuery, RequestResult<UserModel>>
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

		public async Task<RequestResult<UserModel>> Handle(LoginQuery request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null)
			{
				return new RequestResult<UserModel>
				{
					Succeeded = false,
					Status = HttpStatusCode.Unauthorized
				};
			}

			var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

			if (result.Succeeded)
			{
				return new RequestResult<UserModel>
                {
					Data = new UserModel
					{
						Avatar = user.Avatar,
						DisplayName = user.DisplayName,
						Token = _jwtGenerator.CreateToken(user),
						Name = user.UserName,
						Id = user.Id,
					},
					Status = HttpStatusCode.OK,
					Succeeded = true
				};
			}
			return new RequestResult<UserModel>
			{
				Succeeded = false,
				Status = HttpStatusCode.Unauthorized,
			};
		}
	}
}
