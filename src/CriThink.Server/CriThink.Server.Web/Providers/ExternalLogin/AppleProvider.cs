using System;
using System.Threading.Tasks;
using CriThink.Common.HttpRepository;
using CriThink.Server.Web.Interfaces;
using CriThink.Server.Web.Models.DTOs;
using Microsoft.Extensions.Configuration;

namespace CriThink.Server.Web.Providers.ExternalLogin
{
    public class AppleProvider : IExternalLoginProvider
    {
        private readonly IRestRepository _restRepository;
        private readonly IConfiguration _configuration;

        public AppleProvider(IRestRepository restRepository, IConfiguration configuration)
        {
            _restRepository = restRepository ?? throw new ArgumentNullException(nameof(restRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public Task<ExternalProviderUserInfo> GetUserAccessInfo(string userToken)
        {
            throw new NotImplementedException();
        }
    }
}