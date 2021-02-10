using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace CriThink.Client.Core.Services
{
    public class CacheGeolocationService : IGeolocationService, IDisposable
    {
        private const string CountryCodeCacheKey = "country_code";

        private readonly IMemoryCache _memoryCache;
        private readonly GeolocationService _geolocationService;

        private bool _isDisposed;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public CacheGeolocationService(IMemoryCache memoryCache, GeolocationService geoService)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _geolocationService = geoService ?? throw new ArgumentNullException(nameof(geoService));
        }

        public async Task<string> GetCurrentCountryCodeAsync()
        {
            if (_memoryCache.TryGetValue(CountryCodeCacheKey, out string cachedValue))
            {
                if (string.IsNullOrWhiteSpace(cachedValue))
                    cachedValue = await _geolocationService.GetCurrentCountryCodeAsync().ConfigureAwait(false);

                _memoryCache.Set(CountryCodeCacheKey, cachedValue);
                return cachedValue;
            }

            return await _memoryCache.GetOrCreateAsync(CountryCodeCacheKey, async entry =>
            {
                entry.SlidingExpiration = CacheDuration;
                return await _geolocationService.GetCurrentCountryCodeAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (disposing)
            {
                _memoryCache?.Dispose();
            }

            _isDisposed = true;
        }

        #endregion
    }
}
