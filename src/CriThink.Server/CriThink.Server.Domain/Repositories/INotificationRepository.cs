using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.QueryResults;

namespace CriThink.Server.Domain.Repositories
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
