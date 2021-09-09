using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Validators;
using CriThink.Server.Core.DomainEvents;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.QueryResults;
using CriThink.Server.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.DomainEventHandlers
{
    internal class NewsSourceNotificationCreateDomainEventHandler : INotificationHandler<NewsSourceNotificationCreateDomainEvent>
    {
        private readonly INewsSourceRepository _newsSourceRepository;
        private readonly IUnknownNewsSourceRepository _unknownNewsSourcesRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<NewsSourceNotificationCreateDomainEventHandler> _logger;

        public NewsSourceNotificationCreateDomainEventHandler(
            INewsSourceRepository newsSourceRepository,
            IUnknownNewsSourceRepository unknownNewsSourcesRepository,
            INotificationRepository notificationRepository,
            ILogger<NewsSourceNotificationCreateDomainEventHandler> logger)
        {
            _newsSourceRepository = newsSourceRepository ??
                throw new ArgumentNullException(nameof(newsSourceRepository));

            _unknownNewsSourcesRepository = unknownNewsSourcesRepository ??
                throw new ArgumentNullException(nameof(unknownNewsSourcesRepository));

            _notificationRepository = notificationRepository ??
                throw new ArgumentNullException(nameof(notificationRepository));

            _logger = logger;
        }

        public async Task Handle(NewsSourceNotificationCreateDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger?.LogInformation(nameof(NewsSourceNotificationCreateDomainEvent));

            var domain = GetDomainFromNewsLink(notification.UnknownNewsSource.Uri);

            var newsSource = await _newsSourceRepository.SearchNewsSourceAsync(domain);
            if (newsSource is null)
                return;

            var unknownNewsSource = notification.UnknownNewsSource;

            await NotifyUsersAsync(
                unknownNewsSource,
                cancellationToken);

            _logger?.LogInformation($"{nameof(NewsSourceNotificationCreateDomainEvent)}: done");
        }

        private async Task NotifyUsersAsync(
            UnknownNewsSource unknownNewsSource,
            CancellationToken cancellationToken)
        {
            const int pageSize = 20;
            var pageIndex = 0;

            do
            {
                IList<GetAllSubscribedUsersQueryResult> subscribedUsers = await _unknownNewsSourcesRepository.GetAllSubscribedUsersAsync(
                    unknownNewsSource.Id,
                    pageSize + 1,
                    pageIndex++,
                    cancellationToken);

                foreach (var user in subscribedUsers.Take(pageSize))
                {
                    await NotifyUserAsync(user.Email, unknownNewsSource.Uri, cancellationToken);
                }

                if (subscribedUsers.Count <= pageSize) break;
            }
            while (true);
        }

        private async Task NotifyUserAsync(
            string userEmail,
            string uri,
            CancellationToken cancellationToken)
        {
            try
            {
                var notificationRequest = await _notificationRepository.GetNotificationByEmailAndLinkAsync(
                    userEmail,
                    uri,
                    cancellationToken);

                notificationRequest.Send();
                await _notificationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                _notificationRepository.DeleteNotificationRequest(notificationRequest);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Can't send an email to user '{userEmail}' for uri '{uri}'");
            }
        }

        private static string GetDomainFromNewsLink(string newsLink)
        {
            var resolver = new DomainValidator();
            return resolver.ValidateDomain(newsLink);
        }
    }
}
