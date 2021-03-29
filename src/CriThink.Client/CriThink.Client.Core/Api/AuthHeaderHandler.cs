using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using MvvmCross;

namespace CriThink.Client.Core.Api
{
    internal class AuthHeaderHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var identityService = Mvx.IoCProvider.Resolve<IIdentityService>();
            if (identityService is null)
                throw new InvalidOperationException($"Can't resolve {nameof(IIdentityService)} in {nameof(AuthHeaderHandler)}");

            var token = await identityService.GetUserTokenAsync().ConfigureAwait(false);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
