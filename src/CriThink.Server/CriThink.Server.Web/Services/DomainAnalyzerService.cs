using System;
using CriThink.Server.Web.Models.DTOs.NewsAnalyzer.Responses;

namespace CriThink.Server.Web.Services
{
    public class DomainAnalyzerService : IDomainAnalyzerService
    {
        public HttpsSupportResponse HasUriHttpsSupport(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return new HttpsSupportResponse
            {
                IsSecure = uri.Scheme == Uri.UriSchemeHttps
            };
        }
    }

    /// <summary>
    /// Offers API to analyze URL's domains
    /// </summary>
    public interface IDomainAnalyzerService
    {
        /// <summary>
        /// Returns a status to identify if the provided URI has HTTPS support or not
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>A response indicating the result</returns>
        HttpsSupportResponse HasUriHttpsSupport(Uri uri);
    }
}