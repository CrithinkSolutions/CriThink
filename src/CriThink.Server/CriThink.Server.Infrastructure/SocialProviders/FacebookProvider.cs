using System;
using System.Threading.Tasks;
using CriThink.Server.Core.Models.DTOs;
using CriThink.Server.Core.Models.DTOs.Facebook;
using CriThink.Server.Core.Models.LoginProviders;
using CriThink.Server.Infrastructure.Api;
using Microsoft.Extensions.Configuration;

namespace CriThink.Server.Infrastructure.SocialProviders
{
    public class FacebookProvider : IExternalLoginProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IFacebookApi _facebookApi;

        public FacebookProvider(IConfiguration configuration, IFacebookApi facebookApi)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _facebookApi = facebookApi ?? throw new ArgumentNullException(nameof(facebookApi));
        }

        public async Task<ExternalProviderUserInfo> GetUserAccessInfo(string userToken)
        {
            var accessToken = _configuration["FacebookApiKey"];

            FacebookTokenResponse debugTokenResponse = await _facebookApi.ValidateTokenAsync(userToken, accessToken)
                .ConfigureAwait(false);

            if (!debugTokenResponse.Data.IsValid)
                throw new InvalidOperationException("The given token is wrong or expired");

            FacebookUserInfoDetail userInfoDetail = await _facebookApi.GetUserDetailsAsync(debugTokenResponse.Data.UserId, accessToken);

            return new ExternalProviderUserInfo
            {
                FirstName = userInfoDetail.FirstName,
                LastName = userInfoDetail.LastName,
                UserId = userInfoDetail.Id,
                Email = userInfoDetail.Email,
                Username = userInfoDetail.Email,
            };
        }
    }
}
