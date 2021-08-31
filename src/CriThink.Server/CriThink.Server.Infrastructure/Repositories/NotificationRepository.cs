using System;
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

        public async Task AddNotificationRequestAsync(UnknownNewsSourceNotificationRequest request, CancellationToken cancellationToken = default)
        {
            await _dbContext.UnknownNewsSourceNotificationRequests.AddAsync(request, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteNotificationRequestAsync(string validated, string userEmail, CancellationToken cancellationToken)
        {
            var notificationRequest = await _dbContext.UnknownNewsSourceNotificationRequests
                .GetNotificationRequestByEmailAndLiknAsync(userEmail, validated, cancellationToken);

            _dbContext.UnknownNewsSourceNotificationRequests.Remove(notificationRequest);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
