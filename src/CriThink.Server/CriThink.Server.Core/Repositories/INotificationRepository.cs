using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.QueryResults;

namespace CriThink.Server.Core.Repositories
{
    public interface INotificationRepository
    {
        Task<IList<GetAllSubscribedUsersWithSourceQueryResult>> GetAllNotificationsAsync(
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task AddNotificationRequestAsync(
            UnknownNewsSourceNotificationRequest request,
            CancellationToken cancellationToken = default);

        Task DeleteNotificationRequestAsync(
            string validated,
            string userEmail,
            CancellationToken cancellationToken);
    }
}
