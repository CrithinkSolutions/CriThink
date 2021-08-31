using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand>
    {
        private readonly IUserProfileRepository _userProfileRepo;
        private readonly ILogger<UpdateUserProfileCommandHandler> _logger;

        public UpdateUserProfileCommandHandler(
            IUserProfileRepository userProfileRepo,
            ILogger<UpdateUserProfileCommandHandler> logger)
        {
            _userProfileRepo = userProfileRepo ??
                throw new ArgumentNullException(nameof(userProfileRepo));

            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("Updating UserProfile", request.UserId);

            var userProfile = await _userProfileRepo.GetUserProfileByUserIdAsync(request.UserId);
            if (userProfile is null)
            {
                _logger?.LogError("Updating UserProfile: UserId not found", request.UserId);
                throw new ResourceNotFoundException(nameof(userProfile));
            }

            userProfile.UpdateFamilyName(request.FamilyName);
            userProfile.UpdateGivenName(request.GivenName);
            userProfile.UpdateDescription(request.Description);
            userProfile.UpdateGender(request.Gender);
            userProfile.UpdateCountry(request.Country);
            userProfile.UpdateTelegram(request.Telegram);
            userProfile.UpdateSkype(request.Skype);
            userProfile.UpdateTwitter(request.Twitter);
            userProfile.UpdateInstagram(request.Instagram);
            userProfile.UpdateFacebook(request.Facebook);
            userProfile.UpdateSnapchat(request.Snapchat);
            userProfile.UpdateYoutube(request.Youtube);
            userProfile.UpdateBlog(request.Blog);
            userProfile.UpdateDateOfBirth(request.DateOfBirth);

            _userProfileRepo.SaveUserProfileAsync(userProfile);

            // TODO: not sure
            await _userProfileRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            _logger?.LogInformation("Updating UserProfile: done", request.UserId);

            return Unit.Value;
        }
    }
}
