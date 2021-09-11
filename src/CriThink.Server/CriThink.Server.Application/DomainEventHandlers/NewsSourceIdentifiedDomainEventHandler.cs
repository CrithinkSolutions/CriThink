using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.DomainEvents;
using CriThink.Server.Providers.EmailSender.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.DomainEventHandlers
{
    internal class NewsSourceIdentifiedDomainEventHandler : INotificationHandler<NewsSourceIdentifiedDomainEvent>
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly ILogger<NewsSourceIdentifiedDomainEventHandler> _logger;

        public NewsSourceIdentifiedDomainEventHandler(
            IEmailSenderService emailSenderService,
            ILogger<NewsSourceIdentifiedDomainEventHandler> logger)
        {
            _emailSenderService = emailSenderService ??
                throw new ArgumentNullException(nameof(emailSenderService));

            _logger = logger;
        }

        public async Task Handle(NewsSourceIdentifiedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger?.LogInformation(nameof(NewsSourceIdentifiedDomainEvent));

            await _emailSenderService.SendIdentifiedNewsSourceEmailAsync(
                notification.Email,
                notification.UnknownNewsSource.Uri,
                notification.UnknownNewsSource.Authenticity.ToString());

            _logger?.LogInformation($"{nameof(NewsSourceIdentifiedDomainEvent)}: done");
        }
    }
}
