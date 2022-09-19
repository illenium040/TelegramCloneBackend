using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR.Handlers.Login;
using MediatR.Handlers.Models;
using MidiatRHandlers.Register;
using MidiatRHandlers;
using Database.Repositories;
using Database.Models;
using Database.Models.DTO;

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
        public async Task<RequestResult<UserModel>> LoginAsync(LoginQuery query) => await _mediator.Send(query);

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<RequestResult> RegisterAsync(RegisterQuery query) => await _mediator.Send(query);

        [HttpGet("search/{userName}")]
        public async Task<RequestResult<IEnumerable<UserDTO>>> SearchAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return null;
            
            var users = _userRepository.Seacrh(userName)
            .Select(x => new UserDTO
            {
                Id = x.Id,
                Name = x.DisplayName,
                LoginName = x.UserName,
                Email = x.Email,
                Avatar = x.Avatar
            }).ToList();
            return new RequestResult<IEnumerable<UserDTO>>
            {
                Data = users,
                Status = System.Net.HttpStatusCode.OK
            };
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
