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

namespace CriThink.Server.Core.Services
{
    public class UnknownNewsSourceService : IUnknownNewsSourceService
    {
        private readonly IMediator _mediator;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IMapper _mapper;

        public UnknownNewsSourceService(IMediator mediator, IEmailSenderService emailSenderService, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _emailSenderService = emailSenderService ?? throw new ArgumentNullException(nameof(emailSenderService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task RequestNotificationForUnknownNewsSourceAsync(NewsSourceNotificationForUnknownDomainRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var getIdCommand = new GetUnknownNewsSourceIdQuery(request.Uri);
            var unknownNewsId = await _mediator.Send(getIdCommand).ConfigureAwait(false);

            var createCommand = new CreateUnknownSourceNotificationRequestCommand(unknownNewsId, request.Email);
            await _mediator.Send(createCommand).ConfigureAwait(false);
        }

        public async Task TriggerUpdateForIdentifiedNewsSourceAsync(TriggerUpdateForIdentifiedNewsSourceRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            if (request.Classification == NewsSourceClassification.Unknown)
                throw new InvalidOperationException("You can't notify a user with an unknown source");

            var getIdCommand = new GetUnknownNewsSourceIdQuery(request.Uri);
            var unknownNewsId = await _mediator.Send(getIdCommand).ConfigureAwait(false);

            int pageSize = 20, pageIndex = 0;

            do
            {
                var subscribedUsersQuery = new GetAllSubscribedUsersQuery(unknownNewsId, pageSize + 1, pageIndex++);
                var subscribedUsers = await _mediator.Send(subscribedUsersQuery).ConfigureAwait(false);

                foreach (var user in subscribedUsers.Take(pageSize))
                {
                    await _emailSenderService.SendIdentifiedNewsSourceEmailAsync(user.Email, request.Uri, request.Classification.ToString())
                        .ConfigureAwait(false);

                    var notifyUserCommand = new RemoveNotifiedUserCommand(user.Id);
                    _ = _mediator.Send(notifyUserCommand);
                }

                if (subscribedUsers.Count <= pageSize) break;
            }
            while (true);

            var authenticity = _mapper.Map<NewsSourceClassification, NewsSourceAuthenticity>(request.Classification);

            var updateIdentifiedNewsSourceCommand = new UpdateIdentifiedNewsSourceCommand(unknownNewsId, authenticity);
            await _mediator.Send(updateIdentifiedNewsSourceCommand).ConfigureAwait(false);

            var createNewsSourceCommand = new CreateNewsSourceCommand(new Uri(request.Uri), authenticity);
            await _mediator.Send(createNewsSourceCommand).ConfigureAwait(false);
        }

        public async Task<UnknownNewsSourceResponse> GetUnknownNewsSourceAsync(Guid unknownNewsSourceId)
        {
            var query = new GetUnknownNewsSourceQuery(unknownNewsSourceId);
            var unknownNewsSource = await _mediator.Send(query).ConfigureAwait(false);

            var response = _mapper.Map<UnknownNewsSource, UnknownNewsSourceResponse>(unknownNewsSource);

            return response;
        }
    }
}
