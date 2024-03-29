﻿using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using Refit;

namespace CriThink.Client.Core.Api
{
    public interface IIdentityApi
    {
        [Post("/" + EndpointConstants.IdentityLogin)]
        Task<UserLoginResponse> LoginAsync([Body] UserLoginRequest request, CancellationToken cancellationToken = default);

        [Post("/" + EndpointConstants.IdentityRefreshToken)]
        Task<UserRefreshTokenResponse> ExchangeTokensAsync([Body] UserRefreshTokenRequest request, CancellationToken cancellationToken = default);

        [Post("/" + EndpointConstants.IdentityForgotPassword)]
        Task RequestTemporaryTokenAsync([Body] ForgotPasswordRequest request, CancellationToken cancellationToken = default);

        [Post("/" + EndpointConstants.IdentityResetPassword)]
        Task<VerifyUserEmailResponse> ResetPasswordAsync([Body] ResetPasswordRequest request, CancellationToken cancellationToken = default);

        [Multipart]
        [Post("/" + EndpointConstants.IdentitySignUp)]
        Task<UserSignUpResponse> SignUpAsync(
            string username,
            string email,
            string password,
            [AliasAs("formFile")] StreamPart streamPart,
            [Header("Accept-Language")] string languages,
            CancellationToken cancellationToken = default);

        [Post("/" + EndpointConstants.Mobile + EndpointConstants.IdentityConfirmEmail)]
        Task<VerifyUserEmailResponse> ConfirmEmailAsync([Body] EmailConfirmationRequest request, CancellationToken cancellationToken = default);

        [Patch("/" + EndpointConstants.IdentityRestoreUser)]
        Task RestoreUserAsync([Body] RestoreUserRequest request, CancellationToken cancellationToken = default);

        [Get("/" + EndpointConstants.IdentityUsernameAvailability)]
        Task<UsernameAvailabilityResponse> GetUsernameAvailabilityAsync([Query] UsernameAvailabilityRequest request, CancellationToken cancellationToken = default);
    }
}