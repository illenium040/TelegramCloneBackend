using DatabaseLayer.Models;
using DatabaseLayer.Repositories;
using MediatR;
using MediatR.Handlers.Models;
using Microsoft.AspNetCore.Identity;

namespace MidiatRHandlers.Register
{
    public class RegisterHandler : IRequestHandler<RegisterQuery, RequestResult>
    {
        private readonly UserManager<User> _userManager;
        public RegisterHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RequestResult> Handle(RegisterQuery request, CancellationToken cancellationToken)
        {
            var result = await _userManager.CreateAsync(new User()
            {
                DisplayName = request.DisplayName,
                UserName = request.DisplayName,
                Email = request.Email,
            }, request.Password);

            if (result.Succeeded) return new RequestResult
            {
                Succeeded = true,
                Status = System.Net.HttpStatusCode.Created
            };

            return new RequestResult
            {
                Errors = result.Errors.Select(x => x.Description),
                Succeeded = result.Succeeded,
                Status = System.Net.HttpStatusCode.OK
            };
        }
    }
}
