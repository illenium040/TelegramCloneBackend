using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DatabaseLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace MediatR.JWT
{
    public class DefaultJwtGenerator : IJwtGenerator
	{
		private readonly SymmetricSecurityKey _key;
		public DefaultJwtGenerator(IConfiguration config)
		{
#if DEBUG
			var token = config["TokenKey"];
#else
var token = config["TokenKeyServer"];
#endif
			_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token));
		}

		public string CreateToken(User user)
		{
			var claims = new List<Claim> { 
				new Claim(JwtRegisteredClaimNames.Name, user.UserName),
				new Claim(JwtRegisteredClaimNames.NameId, user.Id)
			};

			var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(7),
				SigningCredentials = credentials
			};
			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		} 
    }
}
