using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR.Handlers;
using MediatR.Handlers.Login;
using MediatR.Handlers.Models;
using MidiatRHandlers.Register;
using MidiatRHandlers;
using Database.Repositories;
using Database.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace TGBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IMediator _mediator;
        private UserRepository _userRepository;
        public UserController(IMediator mr, UserRepository userRepository)
        {
            _mediator = mr;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<RequestResult<UserModel>> LoginAsync(LoginQuery query)
        {
            return await _mediator.Send(query);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<RequestResult> RegisterAsync(RegisterQuery query)
        {
            return await _mediator.Send(query);
        }

#if DEBUG
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetUsers();
        }
#endif

    }
}
