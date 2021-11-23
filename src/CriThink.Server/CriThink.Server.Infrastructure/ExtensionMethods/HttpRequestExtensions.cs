using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Infrastructure.ExtensionMethods
{
    public static class HttpRequestExtensions
    {
        public static string GetLanguageFromRequest(
            this HttpRequest request,
            RequestLocalizationOptions localizationOptions)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            if (localizationOptions is null)
                throw new ArgumentNullException(nameof(localizationOptions));

            string languageFilter = null;

            var hasLanguage = request.Headers.TryGetValue("Accept-Language", out var languageValues);
            if (hasLanguage && languageValues.Any())
{
                var commonLanguages = localizationOptions.SupportedCultures
                    .Select(sc => sc.TwoLetterISOLanguageName.ToLowerInvariant())
                    .Intersect(languageValues)
                    .ToList();

                if (commonLanguages.Any())
                {
                    languageFilter = commonLanguages
                            .Aggregate((i, j) => $"{i},{j}");
                }
            }

            return languageFilter;
        }
    }
}
