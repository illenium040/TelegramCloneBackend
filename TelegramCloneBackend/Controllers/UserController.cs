using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR.Handlers.Login;
using MediatR.Handlers.Models;
using CQRSLayer.Register;
using CQRSLayer;
using DatabaseLayer.Repositories;
using DatabaseLayer.Models;
using DatabaseLayer.Models.DTO;
using DatabaseLayer.Repositories.Base;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using DatabaseLayer.Models.Extensions;

namespace TGBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        public UserController(IMediator mr, IUserRepository userRepository)
        {
            _mediator = mr;
            _userRepository = userRepository;
        }

        [HttpPost("validate")]
        public async Task<CommandResult> IsValid()
        {
            var jwt = (SecurityToken)HttpContext.Items["JWT"];
            if (jwt == null) return CommandResult.NotAuthorize();
            if (jwt.ValidTo <= DateTime.Now.AddMinutes(10)) return CommandResult.NotAuthorize();

            var token = (JwtSecurityToken)jwt;
            var id = token.Claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value;
            var name = token.Claims.First(x => x.Type == JwtRegisteredClaimNames.Name).Value;

            var user = _userRepository.Get(id);
            //what if name exists?

            return user == null ? CommandResult.NotAuthorize() : CommandResult.OK();

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<CommandResult<UserModel>> LoginAsync(LoginCommand query) => await _mediator.Send(query);

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<CommandResult> RegisterAsync(RegisterCommand query) => await _mediator.Send(query);

        [HttpGet("search/{userName}")]
        public async Task<CommandResult<IEnumerable<UserDTO>>> SearchAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return null;
            
            var users = _userRepository
                .Seacrh(userName)
                .Select(x => x.ToDTO()).ToList();
            return new CommandResult<IEnumerable<UserDTO>>
            {
                Data = users,
                Result = CommandResult.OK()
            };
        }


#if DEBUG
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<User> GetAllUsers() => _userRepository.GetAll();
#endif

    }
}
