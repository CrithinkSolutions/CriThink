using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.UnknownNewsSource.Requests;

namespace CriThink.Server.Core.Interfaces
{
    public interface IUnknownNewsSourceService
    {

        /// <summary>
        /// Register the user in order to be notified when a specified unknown news source will be analyzed
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task RequestNotificationForUnknownNewsSourceAsync(NewsSourceNotificationForUnknownDomainRequest request);

        /// <summary>
        /// Trigger an update in order to send an email to each subscribed user for the specified unknown news source
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task TriggerUpdateForUnknownNewsSourceAsync(TriggerUpdateForUnknownNewsSourceRequest request);
    }
}
