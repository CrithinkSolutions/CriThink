using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace CriThink.Client.Core.Api
{
    public class CriThinkApiHandler : HttpClientHandler
    {
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
                .OrResult<HttpResponseMessage>(x => !x.IsSuccessStatusCode && ((int) x.StatusCode < 400 || (int) x.StatusCode >= 500))
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromMilliseconds(100 * Math.Pow(2, 3))
                })
                .ExecuteAsync(() =>
                {
                    request.Headers.TryAddWithoutValidation("api-version", "1.0");
                    return base.SendAsync(request, cancellationToken);
                });
    }
}
