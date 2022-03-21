using System.Threading.Tasks;
using CriThink.Server.Infrastructure.SocialProviders;
using Microsoft.AspNetCore.Identity;

namespace CriThink.Server.Infrastructure.Services
{
    public interface IExternalLoginProvider
    {
        Task<ExternalProviderUserInfo> GetUserAccessInfoAsync(
            ExternalLoginInfo loginInfo);
    }
}
