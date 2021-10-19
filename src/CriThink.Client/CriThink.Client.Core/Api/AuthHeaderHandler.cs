using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Exceptions;
using CriThink.Client.Core.Models.Identity;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints;
using MvvmCross;
using Refit;

namespace CriThink.Client.Core.Api
{
    internal class AuthHeaderHandler : DelegatingHandler
    {
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await SemaphoreSlim.WaitAsync(cancellationToken);
            return await SendRequestAsync(request, cancellationToken);
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                return await EnsureAuthHeaderIsValidAsync(request, cancellationToken);
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }

        private async Task<HttpResponseMessage> EnsureAuthHeaderIsValidAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var identityService = ResolveIdentityService();

            var currentUser = await GetUserAccessAsync(identityService).ConfigureAwait(false);
            var token = currentUser?.JwtToken;
            if (token is null)
                return await TryMakeRequestAsync(request, cancellationToken).ConfigureAwait(false);

            var remainintLivingTime = token.ExpirationDate - DateTime.UtcNow;
            if (remainintLivingTime < TimeSpan.FromMinutes(20))
            {
                await HandleTokensRenewalAsync(identityService, request, cancellationToken);
            }
            else
            {
                UpdateRequestToken(request, token.Token);
            }

            var responseMessage = await TryMakeRequestAsync(request, cancellationToken).ConfigureAwait(false);

            // If the server has changed the expiration time and the client does not know..
            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                await HandleTokensRenewalAsync(identityService, request, cancellationToken);
                responseMessage = await TryMakeRequestAsync(request, cancellationToken).ConfigureAwait(false);

                // Our refresh token has been removed from DB. User needs to re enter credentials
                if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new TokensExpiredException();
                }
            }

            return responseMessage;
        }

        private Task<HttpResponseMessage> TryMakeRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken);
        }

        private static Task<UserAccess> GetUserAccessAsync(IIdentityService identityService) =>
            identityService.GetLoggedUserAccessAsync();

        private static async Task HandleTokensRenewalAsync(IIdentityService identityService, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var newData = await identityService.ExchangeTokensAsync(cancellationToken);
                UpdateRequestToken(request, newData.JwtToken.Token);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Forbidden &&
                    ex.Headers.Contains(HeadersConstants.RefreshTokenExpired))
                {
                    throw new TokensExpiredException();
                }
            }
        }

        private static IIdentityService ResolveIdentityService()
        {
            return Mvx.IoCProvider.Resolve<IIdentityService>() ??
                   throw new InvalidOperationException($"Can't resolve {nameof(IIdentityService)} in {nameof(AuthHeaderHandler)}");
        }

        private static void UpdateRequestToken(HttpRequestMessage request, string token)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
