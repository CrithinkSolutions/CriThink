using System;
using System.Net.Http;
using System.Threading.Tasks;
using CriThink.Common.Helpers;
using CriThink.Server.Infrastructure.Api;
using CriThink.Server.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.SocialProviders
{
    internal class GoogleProvider : IExternalLoginProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IGoogleApi _googleApi;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<GoogleProvider> _logger;

        public GoogleProvider(IConfiguration configuration, IGoogleApi googleApi, IHttpClientFactory clientFactory, ILogger<GoogleProvider> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _googleApi = googleApi ?? throw new ArgumentNullException(nameof(googleApi));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _logger = logger;
        }

        public async Task<ExternalProviderUserInfo> GetUserAccessInfo(string userToken)
        {
            var appSecret = _configuration["GoogleApiKey"];

            GoogleTokenInfo result = await _googleApi.GetUserDetailsAsync(userToken)
                .ConfigureAwait(false);

            if (result.ApplicationId != appSecret)
                throw new Exception();

            if (!result.EmailVerified)
                throw new Exception();

            if (DateTime.UtcNow > result.ExpiresAtUtc)
                throw new Exception();

            var userInfo = new ExternalProviderUserInfo
            {
                FirstName = result.FirstName,
                LastName = result.LastName,
                Email = result.Email,
                UserId = result.UserId,
                Username = result.Name?.RemoveWhitespaces(),
            };

            if (!string.IsNullOrWhiteSpace(result.Picture))
            {
                try
                {
                    var client = _clientFactory.CreateClient();
                    userInfo.ProfileAvatarBytes = await client.GetByteArrayAsync(result.Picture);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error getting Google user profile");
                }
            }

            return userInfo;
        }
    }
}
