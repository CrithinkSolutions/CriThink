using System;
using System.Threading.Tasks;
using CriThink.Common.HttpRepository;
using CriThink.Server.Core.Models.DTOs;
using CriThink.Server.Core.Models.DTOs.Facebook;
using CriThink.Server.Core.Models.LoginProviders;
using Microsoft.Extensions.Configuration;

namespace CriThink.Server.Infrastructure.SocialProviders
{
    public class FacebookProvider : IExternalLoginProvider
    {
        private readonly IRestRepository _restRepository;
        private readonly IConfiguration _configuration;

        public FacebookProvider(IRestRepository restRepository, IConfiguration configuration)
        {
            _restRepository = restRepository ?? throw new ArgumentNullException(nameof(restRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<ExternalProviderUserInfo> GetUserAccessInfo(string userToken)
        {
            var accessToken = _configuration["FacebookApiKey"];

            var tokenInfoPath = $"debug_token?input_token={userToken}&access_token={accessToken}";

            var debugTokenResponse = await _restRepository.MakeRequestAsync<FacebookTokenResponse>(tokenInfoPath, HttpRestVerb.Get, "Facebook").ConfigureAwait(false);

            if (!debugTokenResponse.Data.IsValid)
                throw new InvalidOperationException("The given token is wrong or expired");

            var userInfoPath = $"{debugTokenResponse.Data.UserId}?fields=id,first_name,last_name,picture,email&access_token={userToken}";

            var userInfoDetail = await _restRepository.MakeRequestAsync<FacebookUserInfoDetail>(userInfoPath, HttpRestVerb.Get, "Facebook").ConfigureAwait(false);

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
