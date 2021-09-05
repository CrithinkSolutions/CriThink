﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.QueryResults;
using CriThink.Server.Core.Repositories;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.ExtensionMethods.DbSets;
using CriThink.Server.Infrastructure.Projections;

namespace CriThink.Server.Infrastructure.Repositories
{
    internal class NotificationRepository : INotificationRepository
    {
        private readonly CriThinkDbContext _dbContext;

        public NotificationRepository(
            CriThinkDbContext dbContext)
        {
            _dbContext = dbContext ??
                throw new ArgumentNullException(nameof(dbContext));
        }

        public IUnitOfWork UnitOfWork => _dbContext;

        public async Task<IList<GetAllSubscribedUsersWithSourceQueryResult>> GetAllNotificationsAsync(
            int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var notificationCollection = await _dbContext.UnknownNewsSourceNotificationRequests
                    .GetAllNotificationRequestsAsync(
                pageSize,
                pageIndex,
                UnknownSourceNotificationRequestProjection.GetAllWithSources,
                cancellationToken);

            return notificationCollection;
        }

        public async Task<UnknownNewsSourceNotificationRequest> GetNotificationByEmailAndLinkAsync(
            string userEmail, string link, CancellationToken cancellationToken = default)
        {
            var notificationRequest = await _dbContext.UnknownNewsSourceNotificationRequests
                .GetNotificationRequestByEmailAndLiknAsync(userEmail, link, cancellationToken);

            return notificationRequest;
        }

        public async Task AddNotificationRequestAsync(UnknownNewsSourceNotificationRequest request, CancellationToken cancellationToken = default)
        {
            await _dbContext.UnknownNewsSourceNotificationRequests.AddAsync(request, cancellationToken);
        }

        public void DeleteNotificationRequest(UnknownNewsSourceNotificationRequest notificationRequest)
        {
            _dbContext.UnknownNewsSourceNotificationRequests.Remove(notificationRequest);
        }
    }
}
