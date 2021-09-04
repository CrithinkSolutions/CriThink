using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Helpers;
using CriThink.Server.Application.Commands;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Repositories;
using CriThink.Server.Providers.EmailSender.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class RestoreUserCommandHandler : IRequestHandler<RestoreUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSenderService _emailSender;
        private readonly ILogger<RestoreUserCommandHandler> _logger;

        public RestoreUserCommandHandler(
            IUserRepository userRepository,
            IEmailSenderService emailSender,
            ILogger<RestoreUserCommandHandler> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _emailSender = emailSender ??
                throw new ArgumentNullException(nameof(emailSender));

            _logger = logger;
        }

        public async Task<Unit> Handle(RestoreUserCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("RestoreUser");

            var user = await _userRepository.FindUserAsync(request.Email ?? request.Username);
            if (user is null)
                throw new ResourceNotFoundException("The user doesn't exists", $"User: '{request.Email ?? request.Username}'");

            user.CancelScheduledDeletion();

            await _userRepository.UpdateUserAsync(user);

            var token = await _userRepository.GenerateUserPasswordResetTokenAsync(user);

            var encodedCode = Base64Helper.ToBase64(token);

            await _emailSender.SendPasswordResetEmailAsync(
                user.Email,
                user.Id.ToString(),
                encodedCode,
                user.UserName);

            _logger?.LogInformation("RestoreUser: done");

            return Unit.Value;
        }
    }
}
