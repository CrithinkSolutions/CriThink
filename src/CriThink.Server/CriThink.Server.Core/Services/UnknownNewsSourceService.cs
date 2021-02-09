using System;
using System.Linq;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.UnknownNewsSource.Requests;
using CriThink.Server.Core.Commands;
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

        public UnknownNewsSourceService(IMediator mediator, IEmailSenderService emailSenderService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _emailSenderService = emailSenderService ?? throw new ArgumentNullException(nameof(emailSenderService));
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

        public async Task TriggerUpdateForIdentifiedNewsSourceAsync(TriggerUpdateForIdentifiedNewsSourceRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var getIdCommand = new GetUnknownNewsSourceIdCommand(request.Uri);
            var unknownNewsId = await _mediator.Send(getIdCommand).ConfigureAwait(false);

            int pageSize = 20, pageIndex = 0;

            do
            {
                var getSubscribedUsersCommand = new GetAllSubscribedUsersQuery(unknownNewsId, pageSize + 1, pageIndex++);
                var subscribedUsers = await _mediator.Send(getSubscribedUsersCommand).ConfigureAwait(false);

                foreach (var user in subscribedUsers.Take(pageSize))
                {
                    await _emailSenderService.SendIdentifiedNewsSourceEmailAsync(user.Email, request.Uri).ConfigureAwait(false);

                    var notifyUserCommand = new RemoveNotifiedUserCommand(user.Id);
                    _ = _mediator.Send(notifyUserCommand);
                }

                if (subscribedUsers.Count <= pageSize) break;

            } while (true);

            var updateIdentifiedNewsSourceCommand = new UpdateIdentifiedNewsSourceCommand(unknownNewsId);
            await _mediator.Send(updateIdentifiedNewsSourceCommand).ConfigureAwait(false);
        }
    }
}
