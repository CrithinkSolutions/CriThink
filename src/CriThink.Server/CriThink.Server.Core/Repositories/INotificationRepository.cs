using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.QueryResults;

namespace CriThink.Server.Core.Repositories
{
    public interface INotificationRepository : IRepository<UnknownNewsSourceNotification>
    {
        Task<IList<GetAllSubscribedUsersWithSourceQueryResult>> GetAllNotificationsAsync(
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<UnknownNewsSourceNotification> GetNotificationByEmailAndLinkAsync(
            string userEmail, string link, CancellationToken cancellationToken = default);

        Task AddNotificationRequestAsync(
            UnknownNewsSourceNotification request,
            CancellationToken cancellationToken = default);

        void DeleteNotificationRequest(UnknownNewsSourceNotification notificationRequest);
    }
}
