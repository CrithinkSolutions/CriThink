using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.DomainEvents;
using CriThink.Server.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.DomainEventHandlers
{
    internal class UpdateNewsSourceRepositoryDomainEventHandler : INotificationHandler<UpdateNewsSourceRepositoryDomainEvent>
    {
        private readonly INewsSourceRepository _newsSourceRepository;

        private readonly ILogger<UpdateNewsSourceRepositoryDomainEventHandler> _logger;

        public UpdateNewsSourceRepositoryDomainEventHandler(
            INewsSourceRepository newsSourceRepository,
            ILogger<UpdateNewsSourceRepositoryDomainEventHandler> logger)
        {
            _newsSourceRepository = newsSourceRepository ??
                throw new ArgumentNullException(nameof(newsSourceRepository));

            _logger = logger;
        }

        public async Task Handle(UpdateNewsSourceRepositoryDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger?.LogInformation(nameof(UpdateNewsSourceRepositoryDomainEvent));

            await _newsSourceRepository.AddNewsSourceAsync(notification.Domain, notification.Authenticity);

            _logger?.LogInformation($"{nameof(UpdateNewsSourceRepositoryDomainEvent)}: done");
        }
    }
}
