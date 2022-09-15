﻿using FluentValidation;

namespace MediatR.Handlers.Login
{
	public class LoginQueryValidation : AbstractValidator<LoginQuery>
	{
		public LoginQueryValidation()
		{
			RuleFor(x => x.Email).EmailAddress().NotEmpty();
			RuleFor(x => x.Password).NotEmpty();
		}
	}
}
