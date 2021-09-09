using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UpdateUserCommandHandler> _logger;

        public UpdateUserCommandHandler(
            IUserRepository userRepository,
            ILogger<UpdateUserCommandHandler> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userId = request.Id;

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user is null)
                throw new CriThinkNotFoundException("User not found", userId);

            if (!string.IsNullOrWhiteSpace(request.UserName))
                user.UserName = request.UserName;

            if (request.IsEmailConfirmed != null)
                user.EmailConfirmed = request.IsEmailConfirmed.Value;

            if (request.IsLockoutEnabled != null)
                user.LockoutEnabled = request.IsLockoutEnabled.Value;

            if (request.LockoutEnd != null)
                user.LockoutEnd = request.LockoutEnd;

            await _userRepository.UpdateUserAsync(user);

            return Unit.Value;
        }
    }
}
