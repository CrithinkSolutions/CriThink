using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.QueryResults;

namespace CriThink.Server.Core.Repositories
{
    public interface INotificationRepository : IRepository<UnknownNewsSourceNotificationRequest>
    {
        Task<IList<GetAllSubscribedUsersWithSourceQueryResult>> GetAllNotificationsAsync(
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<UnknownNewsSourceNotificationRequest> GetNotificationByEmailAndLinkAsync(
            string userEmail, string link, CancellationToken cancellationToken = default);

        Task AddNotificationRequestAsync(
            UnknownNewsSourceNotificationRequest request,
            CancellationToken cancellationToken = default);

        void DeleteNotificationRequest(UnknownNewsSourceNotificationRequest notificationRequest);
    }
}
