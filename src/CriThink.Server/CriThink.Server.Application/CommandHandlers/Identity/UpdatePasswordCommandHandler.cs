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
    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UpdatePasswordCommandHandler> _logger;

        public UpdatePasswordCommandHandler(
            IUserRepository userRepository,
            ILogger<UpdatePasswordCommandHandler> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _logger = logger;
        }

        public async Task<Unit> Handle(
            UpdatePasswordCommand request,
            CancellationToken cancellationToken)
        {
            _logger?.LogInformation("UpdatePassword");

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var userId = request.UserId;

            var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
            if (user is null)
                throw new CriThinkNotFoundException("The user doesn't exists", $"User id: '{userId}'");

            if (user.IsDeleted)
                throw new InvalidOperationException("The user is disabled");

            var result = await _userRepository.ChangeUserPasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                var ex = new CriThinkIdentityOperationException(result);
                _logger?.LogError(ex, "Error changing user password", user);
                throw ex;
            }

            _logger?.LogInformation("UpdatePassword: done");

            return Unit.Value;
        }
    }
}
