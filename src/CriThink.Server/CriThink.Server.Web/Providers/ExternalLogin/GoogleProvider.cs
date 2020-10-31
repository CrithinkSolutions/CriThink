using System;
using System.Threading.Tasks;
using CriThink.Common.HttpRepository;
using CriThink.Server.Web.Interfaces;
using CriThink.Server.Web.Models;
using CriThink.Server.Web.Models.DTOs.Google;
using Microsoft.Extensions.Configuration;

namespace CriThink.Server.Web.Providers.ExternalLogin
{
    public class GoogleProvider : IExternalLoginProvider
    {
        private readonly IRestRepository _restRepository;
        private readonly IConfiguration _configuration;

        public GoogleProvider(IRestRepository restRepository, IConfiguration configuration)
        {
            _restRepository = restRepository ?? throw new ArgumentNullException(nameof(restRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<ExternalProviderUserInfo> GetUserAccessInfo(string userToken)
        {
            var path = $"tokeninfo?id_token={userToken}";

            var result = await _restRepository.MakeRequestAsync<TokenInfo>(path, HttpRestVerb.Get, "Google").ConfigureAwait(false);

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