using System;
using System.Threading.Tasks;
using CriThink.Server.Core.Models.DTOs;
using CriThink.Server.Core.Models.DTOs.Google;
using CriThink.Server.Core.Models.LoginProviders;
using CriThink.Server.Infrastructure.Api;
using Microsoft.Extensions.Configuration;

namespace CriThink.Server.Infrastructure.SocialProviders
{
    public class GoogleProvider : IExternalLoginProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IGoogleApi _googleApi;

        public GoogleProvider(IConfiguration configuration, IGoogleApi googleApi)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _googleApi = googleApi;
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

            return new ExternalProviderUserInfo
            {
                FirstName = result.FirstName,
                LastName = result.LastName,
                Email = result.Email,
                UserId = result.UserId,
                Username = result.Email,
            };
        }
    }
}
