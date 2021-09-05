using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.DomainEvents;
using CriThink.Server.Providers.EmailSender.Services;
using MediatR;

namespace CriThink.Server.Application.DomainEventHandlers
{
    internal class NewsSourceIdentifiedDomainEventHandler : INotificationHandler<NewsSourceIdentifiedDomainEvent>
    {
        private readonly IEmailSenderService _emailSenderService;

        public NewsSourceIdentifiedDomainEventHandler(
            IEmailSenderService emailSenderService)
        {
            _emailSenderService = emailSenderService ??
                throw new ArgumentNullException(nameof(emailSenderService));
        }

        public async Task Handle(NewsSourceIdentifiedDomainEvent notification, CancellationToken cancellationToken)
        {
            await _emailSenderService.SendIdentifiedNewsSourceEmailAsync(
                notification.Email,
                notification.UnknownNewsSource.Uri,
                notification.UnknownNewsSource.Authenticity.ToString());
        }
    }
}
