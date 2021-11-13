using System;
using System.Threading.Tasks;
using CriThink.Server.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace CriThink.Server.Infrastructure.SocialProviders
{
    internal class AppleProvider : IExternalLoginProvider
    {
        private readonly IConfiguration _configuration;

        public AppleProvider(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public Task<ExternalProviderUserInfo> GetUserAccessInfo(string userToken)
        {
            throw new NotImplementedException();
        }
    }
}
