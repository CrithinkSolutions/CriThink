using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace CriThink.Client.Core.Handlers
{
    public class CriThinkApiHandler : HttpClientHandler
    {
        private static readonly HashSet<HttpStatusCode>
            DisableRetryPolicyStatusCodes = new HashSet<HttpStatusCode>
            {
                HttpStatusCode.BadRequest,
                HttpStatusCode.Unauthorized
            };

        public CriThinkApiHandler()
        {
            SslProtocols = SslProtocols.Tls12;
            if (base.SupportsAutomaticDecompression)
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            }
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Policy.Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(x => !x.IsSuccessStatusCode && !DisableRetryPolicyStatusCodes.Contains(x.StatusCode))
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(Math.Pow(2, 3))
                })
                .ExecuteAsync(() =>
                {
                    request.Headers.TryAddWithoutValidation("api-version", "1.0");
                    return base.SendAsync(request, cancellationToken);
                });
    }
}
