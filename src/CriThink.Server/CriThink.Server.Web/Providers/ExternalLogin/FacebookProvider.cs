using System;
using System.Threading.Tasks;
using CriThink.Common.HttpRepository;
using CriThink.Server.Core.Entities;
using CriThink.Server.Web.Interfaces;
using CriThink.Server.Web.Models.DTOs;
using CriThink.Server.Web.Models.DTOs.Facebook;
using Microsoft.Extensions.Configuration;

namespace CriThink.Server.Web.Providers.ExternalLogin
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
            var accessToken = _configuration["FacebookAPIKey"];

            var tokenInfoPath = $"debug_token?input_token={userToken}&access_token={accessToken}";
            
            var debugTokenResponse = await _restRepository.MakeRequestAsync<DebugTokenResponse>(tokenInfoPath, HttpRestVerb.Get, "Facebook").ConfigureAwait(false);

            var userInfoPath = $"{debugTokenResponse.Data.UserId}?fields=id,first_name,last_name,picture,gender,email&access_token={accessToken}";

            var userInfoDetail = await _restRepository.MakeRequestAsync<UserInfoDetail>(userInfoPath, HttpRestVerb.Get, "Facebook").ConfigureAwait(false);

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