using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.UnknownNewsSource.Requests;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Queries;
using MediatR;

namespace CriThink.Server.Core.Services
{
    public class UnknownNewsSourceService : IUnknownNewsSourceService
    {
        private readonly IMediator _mediator;

        public UnknownNewsSourceService(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task RequestNotificationForUnknownNewsSourceAsync(NewsSourceNotificationForUnknownDomainRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var getIdCommand = new GetUnknownNewsSourceIdCommand(request.Uri);
            var unknownNewsId = await _mediator.Send(getIdCommand).ConfigureAwait(false);

            var createCommand = new CreateUnknownSourceNotificationRequestCommand(unknownNewsId, request.Email);

            await _mediator.Send(createCommand).ConfigureAwait(false);
        }

        public async Task TriggerUpdateForUnknownNewsSourceAsync(TriggerUpdateForUnknownNewsSourceRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var getIdCommand = new GetUnknownNewsSourceIdCommand(request.Uri);
            var unknownNewsId = await _mediator.Send(getIdCommand).ConfigureAwait(false);

            var getSubscribedUsersCommand = new GetAllSubscribedUsersCommand(unknownNewsId);
            var subscribedUsers = await _mediator.Send(getSubscribedUsersCommand).ConfigureAwait(false);

            foreach (var user in subscribedUsers)
            {
                var notifyUserCommand = new NotifyUserCommand();
            }
        }
    }
}
