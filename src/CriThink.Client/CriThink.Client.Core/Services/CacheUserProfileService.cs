using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Models.Identity;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using Microsoft.Extensions.Caching.Memory;

namespace CriThink.Client.Core.Services
{
    public class CacheUserProfileService : IUserProfileService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly UserProfileService _userProfileService;
        private const string UserProfileKey = "user_profile";

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public CacheUserProfileService(IMemoryCache memoryCache, UserProfileService userProfileService)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _userProfileService = userProfileService ?? throw new ArgumentNullException(nameof(userProfileService));
        }

        public async Task<User> GetUserProfileAsync(CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync($"{UserProfileKey}", async entry =>
            {
                entry.SlidingExpiration = CacheDuration;
                return await _userProfileService.GetUserProfileAsync(cancellationToken).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        public async Task UpdateUserProfileAsync(UserProfileUpdateRequest request, CancellationToken cancellationToken = default)
        {
            await _userProfileService.UpdateUserProfileAsync(request, cancellationToken).ConfigureAwait(false);
            ClearUserInfoFromCache();
        }

        private void ClearUserInfoFromCache()
        {
            _memoryCache.Remove(UserProfileKey);
        }
    }
}