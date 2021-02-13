using System;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Logging;
using Xamarin.Essentials;

namespace CriThink.Client.Core.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly IMvxLog _log;

        public GeolocationService(IMvxLogProvider logProvider)
        {
            _log = logProvider?.GetLogFor<GeolocationService>();
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
                        _log?.Info($"User country: {placemark.CountryCode}");
                        return placemark.CountryCode.ToLowerInvariant();
                    }
                }
            }
            catch (PermissionException ex)
            {
                _log?.ErrorException("Can't get country code because of lack of permissions", ex);
            }
            catch (Exception ex)
            {
                _log?.ErrorException("Can't get country code", ex);
            }

            _log?.Info("Can't get user country");
            return string.Empty;
        }
    }

    public interface IGeolocationService
    {
        Task<string> GetCurrentCountryCodeAsync();
    }
}
