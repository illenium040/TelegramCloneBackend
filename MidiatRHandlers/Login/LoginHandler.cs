using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Rest;
using System.Net;
using DatabaseLayer.Models;
using MediatR.JWT;
using MediatR.Handlers.Models;
using CQRSLayer;

namespace MediatR.Handlers.Login
{
    public class LoginHandler : ICommandHandler<LoginCommand, UserModel>
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

		public async Task<CommandResult<UserModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null)
			{
				return new CommandResult<UserModel>
				{
					Result = CommandResult.NotAuthorize()
				};
			}

			var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

			if (result.Succeeded)
			{
				return new CommandResult<UserModel>
                {
					Data = new UserModel
					{
						Avatar = user.Avatar,
						Token = _jwtGenerator.CreateToken(user),
						DisplayName = user.DisplayName,
						Id = user.Id,
					},
					Result = CommandResult.OK()
				};
			}
			return new CommandResult<UserModel>
			{
				Result = CommandResult.NotAuthorize()	
			};
		}
	}
}
