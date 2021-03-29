using System;
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
                .OrResult<HttpResponseMessage>(x => !x.IsSuccessStatusCode)
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(8), TimeSpan.FromSeconds(15),
                })
                .ExecuteAsync(() =>
                {
                    request.Headers.TryAddWithoutValidation("api-version", "1.0");
                    return base.SendAsync(request, cancellationToken);
                });
    }
}
