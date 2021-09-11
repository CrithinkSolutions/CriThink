using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Domain.Exceptions;
using CriThink.Server.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class ResetUserPasswordCommandHandler : IRequestHandler<ResetUserPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ResetUserPasswordCommandHandler> _logger;

        public ResetUserPasswordCommandHandler(
            IUserRepository userRepository,
            ILogger<ResetUserPasswordCommandHandler> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _logger = logger;
        }

        public async Task<Unit> Handle(ResetUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var token = request.Token;
            var newPassword = request.NewPassword;

            var user = await _userRepository.FindUserAsync(userId, cancellationToken);
            if (user is null)
                throw new CriThinkNotFoundException("The user doesn't exists", $"UserId: '{userId}'");
            if (user.IsDeleted)
                throw new InvalidOperationException("The user is disabled");

            var result = await _userRepository.ResetUserPasswordAsync(user, token, newPassword).ConfigureAwait(false);

            if (!result.Succeeded)
            {
                var ex = new CriThinkIdentityOperationException(result);
                _logger?.LogError(ex, "Error resetting user password", user, token);
            }

            return Unit.Value;
        }
    }
}
