using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.Notification;
using Refit;

namespace CriThink.Client.Core.Api
{
    public interface INotificationApi
    {
        [Post("/" + EndpointConstants.NotificationUnknownNewsSource)]
        Task RequestNotificationForUnknownSourceAsync(
            [Body] NewsSourceNotificationForUnknownDomainRequest request,
            CancellationToken cancellationToken = default);

        [Patch("/" + EndpointConstants.NotificationUnknownNewsSource)]
        Task CancelNotificationForUnknownSourceAsync(
            [Body] NewsSourceCancelNotificationForUnknownDomainRequest request,
            CancellationToken cancellationToken = default);
    }
}
