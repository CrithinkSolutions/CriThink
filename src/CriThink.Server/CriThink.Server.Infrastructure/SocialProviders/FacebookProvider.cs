using System;
using System.Net.Http;
using System.Threading.Tasks;
using CriThink.Common.Helpers;
using CriThink.Server.Domain.Models.DTOs;
using CriThink.Server.Domain.Models.DTOs.Facebook;
using CriThink.Server.Domain.Models.LoginProviders;
using CriThink.Server.Infrastructure.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.SocialProviders
{
    public class FacebookProvider : IExternalLoginProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IFacebookApi _facebookApi;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<FacebookProvider> _logger;

        public FacebookProvider(IConfiguration configuration, IFacebookApi facebookApi, IHttpClientFactory clientFactory, ILogger<FacebookProvider> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _facebookApi = facebookApi ?? throw new ArgumentNullException(nameof(facebookApi));
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<ExternalProviderUserInfo> GetUserAccessInfo(string userToken)
        {
            var accessToken = _configuration["FacebookApiKey"];

            FacebookTokenResponse debugTokenResponse = await _facebookApi.ValidateTokenAsync(userToken, accessToken)
                .ConfigureAwait(false);

            if (!debugTokenResponse.Data.IsValid)
                throw new InvalidOperationException("The given token is wrong or expired");

            FacebookUserInfoDetail userInfoDetail = await _facebookApi.GetUserDetailsAsync(debugTokenResponse.Data.UserId, userToken);

            var userInfo = new ExternalProviderUserInfo
            {
                FirstName = userInfoDetail.FirstName,
                LastName = userInfoDetail.LastName,
                UserId = userInfoDetail.Id,
                Email = userInfoDetail.Email,
                Username = userInfoDetail.Name?.RemoveWhitespaces(),
            };

            if (!string.IsNullOrWhiteSpace(userInfoDetail.Picture?.Data?.Url))
            {
                try
                {
                    var client = _clientFactory.CreateClient();
                    userInfo.ProfileAvatarBytes = await client.GetByteArrayAsync(userInfoDetail.Picture.Data.Url);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error getting Facebook user profile");
                }
            }

            return userInfo;
        }
    }
}
