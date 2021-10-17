using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Providers.EmailSender.Providers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class CleanUpFailedEmailsCommandHandler : IRequestHandler<CleanUpFailedEmailsCommand>
    {
        private readonly IEmailSendingFailureRepository _emailSendingFailureRepository;
        private readonly IEmailSenderProvider _emailSenderProvider;
        private readonly ILogger<CleanUpFailedEmailsCommandHandler> _logger;

        public CleanUpFailedEmailsCommandHandler(
            IEmailSendingFailureRepository emailSendingFailureRepository,
            IEmailSenderProvider emailSenderProvider,
            ILogger<CleanUpFailedEmailsCommandHandler> logger)
        {
            _emailSendingFailureRepository = emailSendingFailureRepository ??
                throw new ArgumentNullException(nameof(emailSendingFailureRepository));

            _emailSenderProvider = emailSenderProvider ??
                throw new ArgumentNullException(nameof(emailSenderProvider));

            _logger = logger;
        }

        public async Task<Unit> Handle(CleanUpFailedEmailsCommand request, CancellationToken cancellationToken)
        {
            var failedEmails = await _emailSendingFailureRepository.GetAllFailuresAsync(cancellationToken);

            foreach (var failedEmail in failedEmails)
            {
                try
                {
                    await _emailSenderProvider.Send(
                        failedEmail.FromAddress,
                        failedEmail.Recipients,
                        failedEmail.Subject,
                        failedEmail.HtmlBody);

                    _emailSendingFailureRepository.RemoveFailure(failedEmail);

                    await _emailSendingFailureRepository.UnitOfWork.SaveEntitiesAsync();
                }
                catch (Exception ex)
                {
                    _logger?.LogCritical(ex, "Error to send email again");
                }
            }

            return Unit.Value;
        }
    }
}
