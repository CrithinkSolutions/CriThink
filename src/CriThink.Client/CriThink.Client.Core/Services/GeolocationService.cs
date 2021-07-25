using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Xamarin.Essentials;

namespace CriThink.Client.Core.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly ILogger<GeolocationService> _logger;

        public GeolocationService(ILogger<GeolocationService> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetCurrentCountryCodeAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync().ConfigureAwait(false);
                if (location != null)
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(location).ConfigureAwait(false);
                    var placemark = placemarks.FirstOrDefault();
                    if (placemark != null)
                    {
                        _logger?.LogInformation($"User country: {placemark.CountryCode}");
                        return placemark.CountryCode.ToLowerInvariant();
                    }
                }
            }
            catch (PermissionException ex)
            {
                _logger?.LogError(ex, "Can't get country code because of lack of permissions");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Can't get country code");
            }

            _logger?.LogInformation("Can't get user country");
            return string.Empty;
        }
    }

    public interface IGeolocationService
    {
        Task<string> GetCurrentCountryCodeAsync();
    }
}
