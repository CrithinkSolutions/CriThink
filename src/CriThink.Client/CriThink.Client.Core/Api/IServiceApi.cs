using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using Refit;

namespace CriThink.Client.Core.Api
{
    public interface IServiceApi
    {
        [Head("/" + EndpointConstants.ServiceAppEnabled)]
        Task IsAppEnabledAsync();
    }
}
