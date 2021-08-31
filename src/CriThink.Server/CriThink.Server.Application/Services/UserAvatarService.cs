using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Constants;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.Services
{
    internal class UserAvatarService : IUserAvatarService
    {
        private readonly IUserProfileRepository _userProfileRepo;
        private readonly IFileService _fileService;
        private readonly ILogger<UserAvatarService> _logger;

        public UserAvatarService(
            IUserProfileRepository userProfileRepo,
            IFileService fileService,
            ILogger<UserAvatarService> logger)
        {
            _userProfileRepo = userProfileRepo ??
                throw new ArgumentNullException(nameof(userProfileRepo));

            _fileService = fileService ??
                throw new ArgumentNullException(nameof(fileService));

            _logger = logger;
        }

        public async Task UpdateUserProfileAvatarAsync(
            Guid userId,
            byte[] bytes,
            CancellationToken cancellationToken = default)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));

            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));

            var userProfile = await _userProfileRepo.GetUserProfileByUserIdAsync(userId);
            if (userProfile is null)
            {
                _logger?.LogError("Updating UserProfile: UserId not found", userId);
                throw new ResourceNotFoundException(nameof(userProfile));
            }

            var uri = await _fileService.SaveFileAsync(
                bytes,
                true,
                ProfileConstants.AvatarFileName,
                userId.ToString(),
                ProfileConstants.ProfileFolder);

            userProfile.UpdateUserAvatar(uri.AbsoluteUri);

            try
            {
                _userProfileRepo.SaveUserProfileAsync(userProfile);

                // TODO: not sure
                await _userProfileRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                _logger?.LogInformation("Updating UserProfile Avatar: done", userId);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Updating UserProfile Avatar: generic error", userId);

                await _fileService.DeleteFileAsync(
                    ProfileConstants.AvatarFileName,
                    userId.ToString(),
                    ProfileConstants.ProfileFolder);

                throw;
            }
        }

        public async Task UpdateUserProfileAvatarAsync(
            Guid userId,
            IFormFile formFile,
            CancellationToken cancellationToken = default)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));

            if (formFile is null)
                throw new ArgumentNullException(nameof(formFile));

            var userProfile = await _userProfileRepo.GetUserProfileByUserIdAsync(userId);
            if (userProfile is null)
            {
                _logger?.LogError("Updating UserProfile: UserId not found", userId);
                throw new ResourceNotFoundException(nameof(userProfile));
            }

            var uri = await _fileService.SaveFileAsync(
                formFile,
                true,
                ProfileConstants.AvatarFileName,
                userId.ToString(),
                ProfileConstants.ProfileFolder);

            userProfile.UpdateUserAvatar(uri.AbsoluteUri);

            try
            {
                _userProfileRepo.SaveUserProfileAsync(userProfile);

                // TODO: not sure
                await _userProfileRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                _logger?.LogInformation("Updating UserProfile Avatar: done", userId);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Updating UserProfile Avatar: generic error", userId);

                await _fileService.DeleteFileAsync(
                    ProfileConstants.AvatarFileName,
                    userId.ToString(),
                    ProfileConstants.ProfileFolder);

                throw;
            }
        }
    }
}
