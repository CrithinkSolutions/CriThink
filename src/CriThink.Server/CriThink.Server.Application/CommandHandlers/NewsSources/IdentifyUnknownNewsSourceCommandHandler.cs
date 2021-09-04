using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Validators;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.QueryResults;
using CriThink.Server.Core.Repositories;
using CriThink.Server.Providers.EmailSender.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class IdentifyUnknownNewsSourceCommandHandler : IRequestHandler<IdentifyUnknownNewsSourceCommand>
    {
        private readonly INewsSourceRepository _newsSourceRepository;
        private readonly IUnknownNewsSourcesRepository _unknownNewsSourcesRepository;
        private readonly IEmailSenderService _emailSenderService;
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<IdentifyUnknownNewsSourceCommandHandler> _logger;

        public IdentifyUnknownNewsSourceCommandHandler(
            INewsSourceRepository newsSourceRepository,
            IUnknownNewsSourcesRepository unknownNewsSourcesRepository,
            IEmailSenderService emailSenderService,
            INotificationRepository notificationRepository,
            ILogger<IdentifyUnknownNewsSourceCommandHandler> logger)
        {
            _newsSourceRepository = newsSourceRepository ??
                throw new ArgumentNullException(nameof(newsSourceRepository));

            _unknownNewsSourcesRepository = unknownNewsSourcesRepository ??
                throw new ArgumentNullException(nameof(unknownNewsSourcesRepository));

            _emailSenderService = emailSenderService ??
                throw new ArgumentNullException(nameof(emailSenderService));

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
            var requestedUri = validator.ValidateDomain(request.Source);

            var newsSourceId = await _unknownNewsSourcesRepository.GetUnknownNewsSourceByIdAsync(
                requestedUri,
                cancellationToken);

            await NotifyUsersAsync(
                newsSourceId,
                request.Source,
                request.Classification.ToString(),
                cancellationToken);


            var newsSource = await _unknownNewsSourcesRepository.GetUnknownNewsSourceByIdAsync(newsSourceId);
            if (newsSource is null)
                throw new ResourceNotFoundException(nameof(newsSource));

            newsSource.UpdateIdentifiedAt(DateTime.UtcNow);
            newsSource.UpdateAuthenticity(request.Classification);

            await ValidateRequestAsync(requestedUri, request.Classification);

            await _newsSourceRepository.AddNewsSourceAsync(requestedUri, request.Classification);

            _logger?.LogInformation($"{nameof(IdentifyUnknownNewsSourceCommand)}: done");

            return Unit.Value;
        }

        private async Task NotifyUsersAsync(
            Guid unknownNewsId,
            string requestedUri,
            string classification,
            CancellationToken cancellationToken)
        {
            const int pageSize = 20;
            var pageIndex = 0;

            do
            {
                IList<GetAllSubscribedUsersQueryResult> subscribedUsers = await _unknownNewsSourcesRepository.GetAllSubscribedUsersAsync
                    (unknownNewsId,
                    pageSize + 1,
                    pageIndex++,
                    cancellationToken);

                foreach (var user in subscribedUsers.Take(pageSize))
                {
                    await NotifyUserAsync(user.Email, requestedUri, classification, cancellationToken);
                }

                if (subscribedUsers.Count <= pageSize) break;
            }
            while (true);
        }

        private async Task NotifyUserAsync(
            string userEmail,
            string uri,
            string classification,
            CancellationToken cancellationToken)
        {
            try
            {
                await _emailSenderService.SendIdentifiedNewsSourceEmailAsync(userEmail, uri, classification);
                await _notificationRepository.DeleteNotificationRequestAsync(userEmail, uri, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Can't send an email to user '{userEmail}' for uri '{uri}'");
            }
        }

        private async Task ValidateRequestAsync(string newsLink, NewsSourceAuthenticity authencity)
        {
            if (authencity != NewsSourceAuthenticity.Suspicious)
                return;

            var existingSource = await _newsSourceRepository.SearchNewsSourceAsync(newsLink)
                    .ConfigureAwait(false);

            if (existingSource is null)
                return;

            var isExistingValid = TryGetExistingNewsSource(existingSource.ToString(), out var existingAuthenticity);
            if (isExistingValid &&
                existingAuthenticity == NewsSourceAuthenticity.Conspiracist ||
                existingAuthenticity == NewsSourceAuthenticity.FakeNews)
            {
                throw new InvalidOperationException($"There is already an existing news source '{newsLink}' marked as '{existingAuthenticity}'");
            }
        }

        private static bool TryGetExistingNewsSource(string newsSource, out NewsSourceAuthenticity existingAuthenticity)
        {
            return Enum.TryParse(newsSource, true, out existingAuthenticity);
        }
    }
}
