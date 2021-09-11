using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Api;
using CriThink.Client.Core.Models.Identity;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using Microsoft.Extensions.Logging;
using Refit;

namespace CriThink.Client.Core.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileApi _userProfileApi;
        private readonly ILogger<UserProfileService> _logger;

        public UserProfileService(IUserProfileApi userProfileApi, ILogger<UserProfileService> logger)
        {
            _userProfileApi = userProfileApi ?? throw new ArgumentNullException(nameof(userProfileApi));
            _logger = logger;
        }

        public async Task<User> GetUserProfileAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _userProfileApi.GetUserDetailsAsync(cancellationToken);
                var user = new User(response);

                return user;
            }
            catch (Exception ex)
            {
                _logger?.LogCritical(ex, "Error occurred getting user profile");
                return null;
            }
        }

        public async Task UpdateUserProfileAsync(UserProfileUpdateRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                await _userProfileApi.UpdateUserProfileAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error updateing user profile");
                throw;
            }
        }

        public async Task UpdateUserProfileAvatarAsync(StreamPart streamPart, CancellationToken cancellationToken = default)
        {
            if (streamPart is null)
                throw new ArgumentNullException(nameof(streamPart));

            try
            {
                await _userProfileApi.UpdateUserProfileAvatarAsync(streamPart, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error updating user avatar");
                throw;
            }
        }
    }
}