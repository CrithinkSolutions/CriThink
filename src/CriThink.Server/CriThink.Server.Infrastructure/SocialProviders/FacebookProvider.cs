﻿using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CriThink.Common.Helpers;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Infrastructure.Api;
using CriThink.Server.Infrastructure.Exceptions;
using CriThink.Server.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.SocialProviders
{
    internal class FacebookProvider : IExternalLoginProvider
    {
        private readonly IFacebookApi _facebookApi;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<FacebookProvider> _logger;

        public FacebookProvider(
            IFacebookApi facebookApi,
            IHttpClientFactory clientFactory,
            ILogger<FacebookProvider> logger)
        {
            _facebookApi = facebookApi ??
                throw new ArgumentNullException(nameof(facebookApi));

            _clientFactory = clientFactory ??
                throw new ArgumentNullException(nameof(clientFactory));

            _logger = logger;
        }

        public async Task<ExternalProviderUserInfo> GetUserAccessInfoAsync(
            ExternalLoginInfo loginInfo)
        {
            if (loginInfo is null)
                throw new ArgumentNullException(nameof(loginInfo));

            var accessToken = loginInfo.AuthenticationTokens?.FirstOrDefault()?.Value;
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new CriThinkInvalidSocialTokenException();

            FacebookUserInfoDetail userInfoDetail = await _facebookApi.GetUserDetailsAsync(
                userId: loginInfo.ProviderKey,
                accessToken: accessToken);

            var userInfo = new ExternalProviderUserInfo
            {
                FirstName = userInfoDetail.FirstName,
                LastName = userInfoDetail.LastName,
                UserId = userInfoDetail.Id,
                Email = userInfoDetail.Email,
                Username = userInfoDetail.Name?.RemoveWhitespaces(),
            };

            if (DateTime.TryParse(userInfoDetail.Birthday, out var birthday))
            {
                userInfo.Birthday = birthday;
            }

            if (Enum.TryParse<Gender>(userInfoDetail.Gender, ignoreCase: true, out var gender))
            {
                userInfo.Gender = gender;
            }

            if (!string.IsNullOrWhiteSpace(userInfoDetail.Location?.Name))
            {
                var separatorIndex = userInfoDetail.Location?.Name.LastIndexOf(",");
                if (separatorIndex.HasValue)
                    userInfo.Country = userInfoDetail.Location?.Name.Substring(separatorIndex.Value + 2);
            }

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
