using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Messenger;
using CriThink.Client.Core.Models.Identity;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using Microsoft.Extensions.Caching.Memory;
using MvvmCross.Plugin.Messenger;
using Refit;

namespace CriThink.Client.Core.Services
{
    public class CacheUserProfileService : IUserProfileService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly UserProfileService _userProfileService;
        private const string UserProfileKey = "user_profile";
        private readonly MvxSubscriptionToken _token;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public CacheUserProfileService(
            IMemoryCache memoryCache,
            UserProfileService userProfileService,
            IMvxMessenger messenger)
        {
            _memoryCache = memoryCache ??
                throw new ArgumentNullException(nameof(memoryCache));

            _userProfileService = userProfileService ??
                throw new ArgumentNullException(nameof(userProfileService));

            _token = messenger?.Subscribe<LogoutPerformedMessage>(OnLogoutPerformed) ??
                throw new ArgumentNullException(nameof(messenger));
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

        public async Task UpdateUserProfileAvatarAsync(StreamPart streamPart, CancellationToken cancellationToken = default)
        {
            await _userProfileService.UpdateUserProfileAvatarAsync(streamPart, cancellationToken).ConfigureAwait(false);
            ClearUserInfoFromCache();
        }

        private void ClearUserInfoFromCache()
        {
            _memoryCache.Remove(UserProfileKey);
        }

        private void OnLogoutPerformed(LogoutPerformedMessage message)
        {
            ClearUserInfoFromCache();
        }
    }
}