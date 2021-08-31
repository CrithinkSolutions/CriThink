using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Validators;
using CriThink.Server.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers.Notification
{
    internal class DeleteNotificationForUnknownSourceCommandHandler : IRequestHandler<DeleteNotificationForUnknownSourceCommand>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<DeleteNotificationForUnknownSourceCommandHandler> _logger;

        public DeleteNotificationForUnknownSourceCommandHandler(
            INotificationRepository notificationRepository,
            ILogger<DeleteNotificationForUnknownSourceCommandHandler> logger)
        {
            _notificationRepository = notificationRepository ??
                throw new ArgumentNullException(nameof(notificationRepository));

            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteNotificationForUnknownSourceCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation(nameof(DeleteNotificationForUnknownSourceCommand));

            var validator = new DomainValidator();
            var validated = validator.ValidateDomain(request.NewsSource);

            await _notificationRepository.DeleteNotificationRequestAsync(
                validated,
                request.UserEmail,
                cancellationToken);

            _logger?.LogInformation($"{nameof(DeleteNotificationForUnknownSourceCommand)}: done");

            return Unit.Value;
        }
    }
}
