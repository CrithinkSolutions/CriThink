using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
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

            var notificationRequest = await _notificationRepository.GetNotificationByEmailAndLinkAsync(
                request.UserEmail,
                request.NewsSource,
                cancellationToken);

            _notificationRepository.DeleteNotificationRequest(notificationRequest);

            await _notificationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            _logger?.LogInformation($"{nameof(DeleteNotificationForUnknownSourceCommand)}: done");

            return Unit.Value;
        }
    }
}
