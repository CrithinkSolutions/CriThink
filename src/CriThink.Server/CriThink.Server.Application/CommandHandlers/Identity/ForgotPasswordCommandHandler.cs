using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.Exceptions;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Providers.EmailSender.Exceptions;
using CriThink.Server.Providers.EmailSender.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IEmailSendingFailureRepository _emailSendingFailureRepository;
        private readonly ILogger<ForgotPasswordCommandHandler> _logger;

        public ForgotPasswordCommandHandler(
            IUserRepository userRepository,
            IEmailSenderService emailSenderService,
            IEmailSendingFailureRepository emailSendingFailureRepository,
            ILogger<ForgotPasswordCommandHandler> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _emailSenderService = emailSenderService ??
                throw new ArgumentNullException(nameof(emailSenderService));

            _emailSendingFailureRepository = emailSendingFailureRepository ??
                throw new ArgumentNullException(nameof(emailSendingFailureRepository));

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

            await SendEmailAsync(user, token, cancellationToken);

            _logger?.LogInformation("ForgotPassword: done");

            return Unit.Value;
        }

        private async Task SendEmailAsync(
            User user,
            string token,
            CancellationToken cancellationToken)
        {
            try
            {
                await _emailSenderService.SendPasswordResetEmailAsync(
                    user.Email,
                    user.Id.ToString(),
                    token,
                    user.UserName);
            }
            catch (CriThinkEmailSendingFailureException ex)
            {
                var failure = EmailSendingFailure.Create(
                    ex.FromAddress,
                    ex.Recipients,
                    ex.HtmlBody,
                    ex.Subject,
                    ex.Message);

                await _emailSendingFailureRepository.AddFailureAsync(failure, cancellationToken);
                await _emailSendingFailureRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }
        }
    }
}
