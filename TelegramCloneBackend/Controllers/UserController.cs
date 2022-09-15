using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR.Handlers;
using MediatR.Handlers.Login;
using MediatR.Handlers.Models;

namespace TGBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IMediator _mediator;
        public UserController(IMediator mr)
        {
            _mediator = mr;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserModel>> LoginAsync(LoginQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetAsync()
        {
            return "Got with JWT";
        }

    }
}
