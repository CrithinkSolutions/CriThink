using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Models.DTOs;
using CriThink.Client.Core.Repositories;
using CriThink.Client.Core.Singletons;
using CriThink.Common.Endpoints;
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

        public async Task<UserLoginResponse> PerformLoginAsync(UserLoginRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var loginResponse = await _restRepository.MakeRequestAsync<BaseResponse<UserLoginResponse>>(
                $"{EndpointConstants.IdentityBase}{EndpointConstants.IdentityLogin}",
                HttpVerb.Post,
                request,
                cancellationToken)
                .ConfigureAwait(false);

            var userLoginResponse = loginResponse.Result;

            LoggedUser.Login(userLoginResponse);

            return userLoginResponse;
        }
    }

    public interface IIdentityService
    {
        /// <summary>
        /// Performs login
        /// </summary>
        /// <param name="request">User data</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Login response data</returns>
        Task<UserLoginResponse> PerformLoginAsync(UserLoginRequest request, CancellationToken cancellationToken);
    }
}
