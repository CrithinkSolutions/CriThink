using System.Threading.Tasks;
using CriThink.Server.Infrastructure.SocialProviders;

namespace CriThink.Server.Infrastructure.Services
{
    public interface IExternalLoginProvider
    {
        Task<ExternalProviderUserInfo> GetUserAccessInfo(string userToken);
    }
}
