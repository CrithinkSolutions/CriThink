using System;
using System.Threading.Tasks;
using CriThink.Client.Core.Repositories;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;

namespace CriThink.Client.Core.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IRestRepository _restRepository;

        public IdentityService(IRestRepository restRepository)
        {
            _restRepository = restRepository ?? throw new ArgumentNullException(nameof(restRepository));
        }

        public async Task PerformLoginAsync(UserLoginRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            await _restRepository.MakeRequestAsync<string>("service/environment", HttpVerb.Get, "default").ConfigureAwait(false);
        }
    }

    public interface IIdentityService
    {
        Task PerformLoginAsync(UserLoginRequest request);
    }
}
