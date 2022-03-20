using System.Threading.Tasks;
using CriThink.Server.Infrastructure.SocialProviders;
using Refit;

namespace CriThink.Server.Infrastructure.Api
{
    internal interface IGoogleApi
    {
        [Get("/tokeninfo?id_token={token}")]
        Task<GoogleTokenInfo> GetUserDetailsAsync(string token);

        [Get("/people/me?personFields=birthdays,genders")]
        Task<GoogleGetMeResponse> GetMeAsync([Authorize] string token);
    }
}