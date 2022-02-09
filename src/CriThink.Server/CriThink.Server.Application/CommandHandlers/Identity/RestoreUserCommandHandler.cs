using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Helpers;
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
    internal class RestoreUserCommandHandler : IRequestHandler<RestoreUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSenderService _emailSender;
        private readonly IEmailSendingFailureRepository _emailSendingFailureRepository;
        private readonly ILogger<RestoreUserCommandHandler> _logger;

        public RestoreUserCommandHandler(
            IUserRepository userRepository,
            IEmailSenderService emailSender,
            IEmailSendingFailureRepository emailSendingFailureRepository,
            ILogger<RestoreUserCommandHandler> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _emailSender = emailSender ??
                throw new ArgumentNullException(nameof(emailSender));

            _emailSendingFailureRepository = emailSendingFailureRepository ??
                throw new ArgumentNullException(nameof(emailSendingFailureRepository));

            _logger = logger;
        }

        public async Task<Unit> Handle(RestoreUserCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("RestoreUser");

            var user = await _userRepository.FindUserAsync(request.Email ?? request.Username);
            if (user is null)
                throw new CriThinkNotFoundException("The user doesn't exists", $"User: '{request.Email ?? request.Username}'");

            user.CancelScheduledDeletion();

            await _userRepository.UpdateUserAsync(user);

            var encodedToken = await _userRepository.GenerateUserPasswordResetTokenAsync(user);

            await SendEmailAsync(user, encodedToken, cancellationToken);

            _logger?.LogInformation("RestoreUser: done");

            return Unit.Value;
        }

        private async Task SendEmailAsync(
            User user,
            string encodedCode,
            CancellationToken cancellationToken)
        {
            try
            {
                await _emailSender.SendPasswordResetEmailAsync(
                    user.Email,
                    user.Id.ToString(),
                    encodedCode,
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
