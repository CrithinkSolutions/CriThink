using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Validators;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.QueryResults;
using CriThink.Server.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class IdentifyUnknownNewsSourceCommandHandler : IRequestHandler<IdentifyUnknownNewsSourceCommand>
    {
        private readonly INewsSourceRepository _newsSourceRepository;
        private readonly IUnknownNewsSourcesRepository _unknownNewsSourcesRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<IdentifyUnknownNewsSourceCommandHandler> _logger;

        public IdentifyUnknownNewsSourceCommandHandler(
            INewsSourceRepository newsSourceRepository,
            IUnknownNewsSourcesRepository unknownNewsSourcesRepository,
            INotificationRepository notificationRepository,
            ILogger<IdentifyUnknownNewsSourceCommandHandler> logger)
        {
            _newsSourceRepository = newsSourceRepository ??
                throw new ArgumentNullException(nameof(newsSourceRepository));

            _unknownNewsSourcesRepository = unknownNewsSourcesRepository ??
                throw new ArgumentNullException(nameof(unknownNewsSourcesRepository));

            _notificationRepository = notificationRepository ??
                throw new ArgumentNullException(nameof(notificationRepository));

            _logger = logger;
        }

        public async Task<Unit> Handle(IdentifyUnknownNewsSourceCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation(nameof(IdentifyUnknownNewsSourceCommand));

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            if (request.Classification == NewsSourceAuthenticity.Unknown)
                throw new InvalidOperationException("You can't notify a user with an unknown source");

            var validator = new DomainValidator();
            var domain = validator.ValidateDomain(request.Source);

            var unknownNewsSource = await _unknownNewsSourcesRepository.GetUnknownNewsSourceByUriAsync(
                request.Source,
                cancellationToken);

            unknownNewsSource.UpdateIdentifiedAt(DateTime.UtcNow);
            unknownNewsSource.UpdateAuthenticity(request.Classification);

            await NotifyUsersAsync(
                unknownNewsSource,
                cancellationToken);

            unknownNewsSource.MarkAsKnown(domain, request.Classification);

            await _unknownNewsSourcesRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            _logger?.LogInformation($"{nameof(IdentifyUnknownNewsSourceCommand)}: done");

            return Unit.Value;
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
    }
}
