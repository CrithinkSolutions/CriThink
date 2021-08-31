using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Core.QueryResults;
using CriThink.Server.Core.Repositories;
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

        public async Task<NotificationRequestGetAllResponse> GetAllNotificationsAsync(int pageSize, int pageIndex)
        {
            _logger?.LogInformation(nameof(GetAllNotificationsAsync));

            var notificationCollection = await _notificationRepository.GetAllNotificationsAsync(pageIndex, pageSize);

            var dtos = notificationCollection
                .Take(pageSize)
                .Select(notification => _mapper.Map<GetAllSubscribedUsersWithSourceQueryResult, NotificationRequestGetResponse>(notification))
                .ToList();

            var response = new NotificationRequestGetAllResponse(dtos, notificationCollection.Count > pageSize);

            _logger?.LogInformation($"{nameof(GetAllNotificationsAsync)}: done");

            return response;
        }
    }
}
