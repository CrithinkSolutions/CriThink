using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.DomainEvents;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Providers.EmailSender.Exceptions;
using CriThink.Server.Providers.EmailSender.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.DomainEventHandlers
{
    internal class NewsSourceIdentifiedDomainEventHandler : INotificationHandler<NewsSourceIdentifiedDomainEvent>
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly IEmailSendingFailureRepository _emailSendingFailureRepository;
        private readonly ILogger<NewsSourceIdentifiedDomainEventHandler> _logger;

        public NewsSourceIdentifiedDomainEventHandler(
            IEmailSenderService emailSenderService,
            IEmailSendingFailureRepository emailSendingFailureRepository,
            ILogger<NewsSourceIdentifiedDomainEventHandler> logger)
        {
            _emailSenderService = emailSenderService ??
                throw new ArgumentNullException(nameof(emailSenderService));

            _emailSendingFailureRepository = emailSendingFailureRepository ??
                throw new ArgumentNullException(nameof(emailSendingFailureRepository));

            _logger = logger;
        }

        public async Task Handle(NewsSourceIdentifiedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger?.LogInformation(nameof(NewsSourceIdentifiedDomainEvent));

            await SendEmailAsync(notification, cancellationToken);

            _logger?.LogInformation($"{nameof(NewsSourceIdentifiedDomainEvent)}: done");
        }

        private async Task SendEmailAsync(
            NewsSourceIdentifiedDomainEvent notification,
            CancellationToken cancellationToken)
        {
            try
            {
                await _emailSenderService.SendIdentifiedNewsSourceEmailAsync(
                notification.Email,
                notification.UnknownNewsSource.Uri,
                notification.UnknownNewsSource.Authenticity.ToString());
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
