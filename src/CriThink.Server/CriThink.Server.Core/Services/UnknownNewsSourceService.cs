using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.NewsSource.Requests;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Interfaces;
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

        public async Task RequestNotificationForUnknownSourceAsync(NewsSourceNotificationForUnknownDomainRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var getIdCommand = new GetUnknownNewsSourceIdCommand(request.Uri);
            var unknownNewsId = await _mediator.Send(getIdCommand).ConfigureAwait(false);

            var createCommand = new CreateUnknownSourceNotificationRequestCommand(unknownNewsId, request.Email);

            await _mediator.Send(createCommand).ConfigureAwait(false);
        }
    }
}
