using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;

namespace CriThink.Client.Core.Services.Contracts
{
    public interface IIdentityService
    {
        Task<string> LoginUserAsync(UserLoginRequest request);
    }
}
