using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Queries;
using CriThink.Server.Application.Validators;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers.Notification
{
    internal class CreateNotificationForUnknownSourceCommandHandler : IRequestHandler<CreateNotificationForUnknownSourceCommand>
    {
        private readonly IUnknownNewsSourceQueries _unknownNewsSourceQueries;
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<CreateNotificationForUnknownSourceCommandHandler> _logger;

        public CreateNotificationForUnknownSourceCommandHandler(
            IUnknownNewsSourceQueries unknownNewsSourceQueries,
            INotificationRepository notificationRepository,
            ILogger<CreateNotificationForUnknownSourceCommandHandler> logger)
        {
            _unknownNewsSourceQueries = unknownNewsSourceQueries ??
                throw new ArgumentNullException(nameof(unknownNewsSourceQueries));

            _notificationRepository = notificationRepository ??
                throw new ArgumentNullException(nameof(notificationRepository));

            _logger = logger;
        }

        public async Task<Unit> Handle(CreateNotificationForUnknownSourceCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation(nameof(CreateNotificationForUnknownSourceCommand));

            var validator = new DomainValidator();
            var validated = validator.ValidateDomain(request.NewsSource);

            var unknownNewsSource = await _unknownNewsSourceQueries.GetUnknownNewsSourceByUriAsync(validated, cancellationToken);

            if (unknownNewsSource is null)
                throw new ResourceNotFoundException($"Can't find an unknown source with url '{request.NewsSource}'");

            var unknownSourcesNotificationRequest = UnknownNewsSourceNotificationRequest.Create(
                request.UserEmail,
                unknownNewsSource);

            await _notificationRepository.AddNotificationRequestAsync(
                unknownSourcesNotificationRequest,
                cancellationToken);

            _logger?.LogInformation($"{nameof(CreateNotificationForUnknownSourceCommand)}: done");

            return Unit.Value;
        }
    }
}
