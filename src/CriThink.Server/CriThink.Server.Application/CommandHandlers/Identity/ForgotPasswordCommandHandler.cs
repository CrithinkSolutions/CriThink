using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Repositories;
using CriThink.Server.Providers.EmailSender.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSenderService _emailSenderService;
        private readonly ILogger<ForgotPasswordCommandHandler> _logger;

        public ForgotPasswordCommandHandler(
            IUserRepository userRepository,
            IEmailSenderService emailSenderService,
            ILogger<ForgotPasswordCommandHandler> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _emailSenderService = emailSenderService ??
                throw new ArgumentNullException(nameof(emailSenderService));

            _logger = logger;
        }

        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("ForgotPassword");

            var user = await _userRepository.FindUserAsync(request.Email ?? request.Username);

            if (user is null)
                throw new CriThinkNotFoundException("The user doesn't exists", $"User email: '{request.Email}' - username: '{request.Username}'");
            if (user.IsDeleted)
                throw new InvalidOperationException("The user is disabled");

            var token = await _userRepository.GenerateUserPasswordResetTokenAsync(user);

            await _emailSenderService.SendPasswordResetEmailAsync(user.Email, user.Id.ToString(), token, user.UserName);

            _logger?.LogInformation("ForgotPassword: done");

            return Unit.Value;
        }
    }
}
