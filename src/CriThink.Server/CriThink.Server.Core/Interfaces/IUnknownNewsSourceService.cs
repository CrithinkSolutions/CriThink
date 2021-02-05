using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.NewsSource.Requests;

namespace CriThink.Server.Core.Interfaces
{
    public interface IUnknownNewsSourceService
    {

        /// <summary>
        /// Register the user in order to be notified when a specified unknown news source will be analyzed
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task RequestNotificationForUnknownSourceAsync(NewsSourceNotificationForUnknownDomainRequest request);
    }
}
