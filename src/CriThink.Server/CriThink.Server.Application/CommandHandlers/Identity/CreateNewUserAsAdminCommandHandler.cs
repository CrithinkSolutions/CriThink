using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class CreateNewUserAsAdminCommandHandler : IRequestHandler<CreateNewUserAsAdminCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<CreateNewUserAsAdminCommandHandler> _logger;

        public CreateNewUserAsAdminCommandHandler(
            IUserRepository userRepository,
            ILogger<CreateNewUserAsAdminCommandHandler> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _logger = logger;
        }

        public async Task<Unit> Handle(CreateNewUserAsAdminCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation(nameof(CreateNewUserAsAdminCommand));

            var user = User.Create(
                request.Username,
                request.Email);

            await _userRepository.CreateUserAsync(user, request.Password);

            var confirmationCode = await _userRepository.GetEmailConfirmationTokenAsync(user);

            await _userRepository.ConfirmUserEmailAsync(user, confirmationCode);

            await _userRepository.AddUserToRoleAsync(user, RoleNames.Admin);

            await _userRepository.AddClaimsToAdminUserAsync(user);

            _logger?.LogInformation($"{nameof(CreateNewUserAsAdminCommand)}: done");

            return Unit.Value;
        }
    }
}
