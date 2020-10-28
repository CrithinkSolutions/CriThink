using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Models.DTOs;
using CriThink.Client.Core.Repositories;
using CriThink.Client.Core.Singletons;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using MvvmCross.Logging;

namespace CriThink.Client.Core.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IRestRepository _restRepository;
        private readonly IMvxLog _logger;

        public IdentityService(IRestRepository restRepository, IMvxLogProvider logProvider)
        {
            _restRepository = restRepository ?? throw new ArgumentNullException(nameof(restRepository));
            _logger = logProvider?.GetLogFor<IdentityService>();
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

        public async Task RequestTemporaryTokenAsync(ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                await _restRepository.MakeRequestAsync(
                        $"{EndpointConstants.IdentityBase}{EndpointConstants.IdentityForgotPassword}",
                        HttpVerb.Post,
                        request,
                        cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.Log(MvxLogLevel.Error, () => "Error requesting temporary token", ex);
            }
        }

        public async Task<VerifyUserEmailResponse> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var resetPasswordResponse = await _restRepository.MakeRequestAsync<BaseResponse<VerifyUserEmailResponse>>(
                    $"{EndpointConstants.IdentityBase}{EndpointConstants.IdentityResetPassword}",
                    HttpVerb.Post,
                    request,
                    cancellationToken)
                .ConfigureAwait(false);

            var userData = resetPasswordResponse.Result;

            LoggedUser.Login(userData);

            return userData;
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

        /// <summary>
        /// Requests a temporary token to restore the password
        /// </summary>
        /// <param name="request">Email or the username of the account owner</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Operation status result</returns>
        Task RequestTemporaryTokenAsync(ForgotPasswordRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Change the current and forgotten password with the given one
        /// </summary>
        /// <param name="request">User id, temporary token and new password</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>User info and token</returns>
        Task<VerifyUserEmailResponse> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken);
    }
}
