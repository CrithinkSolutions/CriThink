using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Endpoints.DTOs.UnknownNewsSource;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Queries;
using CriThink.Server.Providers.EmailSender.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Core.Services
{
    public class UnknownNewsSourceService : IUnknownNewsSourceService
    {
        private readonly IMediator _mediator;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IMapper _mapper;
        private readonly ILogger<UnknownNewsSourceService> _logger;

        public UnknownNewsSourceService(IMediator mediator, IEmailSenderService emailSenderService, IMapper mapper, ILogger<UnknownNewsSourceService> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _emailSenderService = emailSenderService ?? throw new ArgumentNullException(nameof(emailSenderService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }

        public async Task RequestNotificationForUnknownNewsSourceAsync(NewsSourceNotificationForUnknownDomainRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var createCommand = new CreateUnknownSourceNotificationRequestCommand(request.Uri, request.Email);
            await _mediator.Send(createCommand).ConfigureAwait(false);
        }

        public async Task TriggerUpdateForIdentifiedNewsSourceAsync(TriggerUpdateForIdentifiedNewsSourceRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            if (request.Classification == NewsSourceClassification.Unknown)
                throw new InvalidOperationException("You can't notify a user with an unknown source");

            var requestedUri = request.Uri;

            var getIdCommand = new GetUnknownNewsSourceIdQuery(requestedUri);
            var unknownNewsId = await _mediator.Send(getIdCommand).ConfigureAwait(false);

            await NotifyUsersAsync(unknownNewsId, requestedUri, request.Classification.ToString())
                .ConfigureAwait(false);

            var authenticity = _mapper.Map<NewsSourceClassification, NewsSourceAuthenticity>(request.Classification);

            var updateIdentifiedNewsSourceCommand = new UpdateIdentifiedNewsSourceCommand(unknownNewsId, authenticity);
            await _mediator.Send(updateIdentifiedNewsSourceCommand).ConfigureAwait(false);

            var createNewsSourceCommand = new CreateNewsSourceCommand(new Uri(requestedUri), authenticity);
            await _mediator.Send(createNewsSourceCommand).ConfigureAwait(false);
        }

        public async Task<UnknownNewsSourceResponse> GetUnknownNewsSourceAsync(Guid unknownNewsSourceId)
        {
            var query = new GetUnknownNewsSourceQuery(unknownNewsSourceId);
            var unknownNewsSource = await _mediator.Send(query).ConfigureAwait(false);

            var response = _mapper.Map<UnknownNewsSource, UnknownNewsSourceResponse>(unknownNewsSource);

            return response;
        }

        private async Task NotifyUsersAsync(Guid unknownNewsId, string requestedUri, string classification)
        {
            const int pageSize = 20;
            var pageIndex = 0;

            do
            {
                var subscribedUsersQuery = new GetAllSubscribedUsersQuery(unknownNewsId, pageSize + 1, pageIndex++);
                var subscribedUsers = await _mediator.Send(subscribedUsersQuery).ConfigureAwait(false);

                foreach (var user in subscribedUsers.Take(pageSize))
                {
                    await NotifyUserAsync(user.Id, user.Email, requestedUri, classification)
                        .ConfigureAwait(false);
                }

                if (subscribedUsers.Count <= pageSize) break;
            }
            while (true);
        }

        private async Task NotifyUserAsync(Guid userId, string userEmail, string uri, string classification)
        {
            try
            {
                await _emailSenderService.SendIdentifiedNewsSourceEmailAsync(userEmail, uri, classification)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Can't send an email to user '{userEmail}' for uri '{uri}'");
                return;
            }

            var notifyUserCommand = new RemoveNotifiedUserCommand(userId);
            await _mediator.Send(notifyUserCommand)
                .ConfigureAwait(false);
        }
    }
}
