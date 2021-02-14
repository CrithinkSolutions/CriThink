using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.UnknownNewsSource;

namespace CriThink.Server.Core.Interfaces
{
    public interface IUnknownNewsSourceService
    {

        /// <summary>
        /// Register the user in order to be notified when a specified unknown news source will be analyzed
        /// </summary>
        /// <param name="request"></param>
        Task RequestNotificationForUnknownNewsSourceAsync(NewsSourceNotificationForUnknownDomainRequest request);

        /// <summary>
        /// Trigger an update in order to send an email to each subscribed user for the specified unknown news source
        /// </summary>
        /// <param name="request"></param>
        Task TriggerUpdateForIdentifiedNewsSourceAsync(TriggerUpdateForIdentifiedNewsSourceRequest request);

        /// <summary>
        /// Get the unknown news source with the given id
        /// </summary>
        /// <param name="unknownNewsSourceId"></param>
        /// <returns>The unknown news source</returns>
        Task<UnknownNewsSourceResponse> GetUnknownNewsSourceAsync(Guid unknownNewsSourceId);
    }
}
