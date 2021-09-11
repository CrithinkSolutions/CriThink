using System.Threading.Tasks;
using CriThink.Server.Domain.Models.DTOs;

namespace CriThink.Server.Domain.Models.LoginProviders
{
    public interface IExternalLoginProvider
    {
        Task<ExternalProviderUserInfo> GetUserAccessInfo(string userToken);
    }
}
