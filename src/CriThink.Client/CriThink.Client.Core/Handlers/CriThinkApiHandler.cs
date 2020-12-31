using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
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

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Version = new Version(2, 0);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
