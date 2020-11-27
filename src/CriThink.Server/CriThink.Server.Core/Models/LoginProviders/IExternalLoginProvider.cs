using System.Threading.Tasks;
using CriThink.Server.Core.Models.DTOs;

namespace CriThink.Server.Core.Models.LoginProviders
{
    public interface IExternalLoginProvider
    {
        Task<ExternalProviderUserInfo> GetUserAccessInfo(string userToken);
    }
}
