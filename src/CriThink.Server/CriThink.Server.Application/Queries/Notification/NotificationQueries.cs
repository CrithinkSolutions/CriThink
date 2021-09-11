using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Server.Application.Administration.ViewModels;
using CriThink.Server.Domain.QueryResults;
using CriThink.Server.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.Queries
{
    internal class NotificationQueries : INotificationQueries
    {
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<NotificationQueries> _logger;

        public NotificationQueries(
            IMapper mapper,
            INotificationRepository notificationRepository,
            ILogger<NotificationQueries> logger)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _notificationRepository = notificationRepository ??
                throw new ArgumentNullException(nameof(notificationRepository));

            _logger = logger;
        }

        public async Task<NotificationRequestGetAllViewModel> GetAllNotificationsAsync(int pageSize, int pageIndex)
        {
            _logger?.LogInformation(nameof(GetAllNotificationsAsync));

            var notificationCollection = await _notificationRepository.GetAllNotificationsAsync(pageIndex, pageSize);

            var dtos = notificationCollection
                .Take(pageSize)
                .Select(notification => _mapper.Map<GetAllSubscribedUsersWithSourceQueryResult, NotificationRequestGetViewModel>(notification))
                .ToList();

            var response = new NotificationRequestGetAllViewModel(dtos, notificationCollection.Count > pageSize);

            _logger?.LogInformation($"{nameof(GetAllNotificationsAsync)}: done");

            return response;
        }
    }
}
