using System.Threading.Tasks;
using CriThink.Server.Infrastructure.SocialProviders;
using Refit;

namespace CriThink.Server.Infrastructure.Api
{
    internal interface IFacebookApi
    {
        [Get("/me?fields=id,first_name,last_name,name,picture,email,gender,birthday,location&access_token={accessToken}")]
        Task<FacebookUserInfoDetail> GetUserDetailsAsync(string userId, string accessToken);
    }
}
