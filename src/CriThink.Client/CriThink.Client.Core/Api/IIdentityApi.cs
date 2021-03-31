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

        [Post("/" + EndpointConstants.IdentityExternalLogin)]
        Task<UserLoginResponse> SocialLoginAsync([Body] ExternalLoginProviderRequest request, CancellationToken cancellationToken = default);

        [Post("/" + EndpointConstants.IdentityForgotPassword)]
        Task RequestTemporaryTokenAsync([Body] ForgotPasswordRequest request, CancellationToken cancellationToken = default);

        [Post("/" + EndpointConstants.IdentityResetPassword)]
        Task<VerifyUserEmailResponse> ResetPasswordAsync([Body] ResetPasswordRequest request, CancellationToken cancellationToken = default);

        [Post("/" + EndpointConstants.IdentitySignUp)]
        Task<UserSignUpResponse> SignUpAsync([Body] UserSignUpRequest request, CancellationToken cancellationToken = default);

        [Post("/" + EndpointConstants.Mobile + EndpointConstants.IdentityConfirmEmail)]
        Task<VerifyUserEmailResponse> ConfirmEmailAsync([Body] EmailConfirmationRequest request, CancellationToken cancellationToken = default);
    }
}