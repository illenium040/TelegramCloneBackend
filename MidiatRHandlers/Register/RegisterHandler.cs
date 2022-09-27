using DatabaseLayer.Models;
using DatabaseLayer.Repositories;
using DatabaseLayer.Repositories.Base;
using MediatR;
using MediatR.Handlers.Models;
using Microsoft.AspNetCore.Identity;

namespace CQRSLayer.Register
{
    public class RegisterHandler : ICommandHandler<RegisterCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserChatRepository _userChatRepository;
        public RegisterHandler(UserManager<User> userManager, IUserChatRepository repository)
        {
            _userManager = userManager;
            _userChatRepository = repository;
        }

        public async Task<CommandResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                DisplayName = request.DisplayName,
                UserName = request.DisplayName,
                Email = request.Email,
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                _userChatRepository.AddChat(user.Id, user.Id);
                return new CommandResult
                {
                    Succeeded = true,
                    Status = System.Net.HttpStatusCode.Created
                };
            }

            return new CommandResult
            {
                Errors = result.Errors.Select(x => x.Description),
                Succeeded = result.Succeeded,
                Status = System.Net.HttpStatusCode.OK
            };
        }
    }
}
