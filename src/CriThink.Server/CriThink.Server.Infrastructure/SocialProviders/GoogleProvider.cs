using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Infrastructure.Api;
using CriThink.Server.Infrastructure.Exceptions;
using CriThink.Server.Infrastructure.ExtensionMethods;
using CriThink.Server.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
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

        public GoogleProvider(
            IConfiguration configuration,
            IGoogleApi googleApi,
            IHttpClientFactory clientFactory,
            ILogger<GoogleProvider> logger)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));

            _googleApi = googleApi ??
                throw new ArgumentNullException(nameof(googleApi));

            _clientFactory = clientFactory ??
                throw new ArgumentNullException(nameof(clientFactory));

            _logger = logger;
        }

        public async Task<ExternalProviderUserInfo> GetUserAccessInfoAsync(
            ExternalLoginInfo loginInfo)
        {
            if (loginInfo is null)
                throw new ArgumentNullException(nameof(loginInfo));

            var userToken = loginInfo.AuthenticationTokens?.FirstOrDefault()?.Value;
            if (string.IsNullOrWhiteSpace(userToken))
                throw new CriThinkInvalidSocialTokenException();

            GoogleGetMeResponse result = await _googleApi.GetMeAsync(userToken);

            DateTime? birthday = null;
            Gender? gender = null;

            var googleDate = result?.Birthdays?.FirstOrDefault()?.Date;
            if (googleDate is not null)
            {
                if (DateTime.TryParse($"{googleDate.Year}-{googleDate.Month}-{googleDate.Day}", out var date))
                {
                    birthday = date;
                }
            }

            var googleGender = result?.Genders?.FirstOrDefault()?.FormattedValue;
            if (googleGender is not null)
            {
                if (Enum.TryParse<Gender>(googleGender, out var parsedGender))
                {
                    gender = parsedGender;
                }
            }

            var userInfo = new ExternalProviderUserInfo
            {
                Birthday = birthday,
                Gender = gender,
            };

            var avatarPath = loginInfo.Principal.GetAvatar();

            if (!string.IsNullOrWhiteSpace(avatarPath))
            {
                try
                {
                    var client = _clientFactory.CreateClient();
                    userInfo.ProfileAvatarBytes = await client.GetByteArrayAsync(avatarPath);
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
