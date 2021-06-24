using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    internal class UpdateUserProfileNewsHandler : IRequestHandler<UpdateUserProfileCommand>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<UpdateUserProfileNewsHandler> _logger;

        public UpdateUserProfileNewsHandler(CriThinkDbContext dbContext, ILogger<UpdateUserProfileNewsHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var updatedUserProfile = request.UserProfile;

            try
            {
                var userProfile = await _dbContext.UserProfiles.SingleOrDefaultAsync(u => u.UserId == updatedUserProfile.UserId, cancellationToken)
                    .ConfigureAwait(false);

                userProfile.FamilyName = updatedUserProfile.FamilyName;
                userProfile.GivenName = updatedUserProfile.GivenName;
                userProfile.Description = updatedUserProfile.Description;
                userProfile.Gender = updatedUserProfile.Gender;
                userProfile.Country = updatedUserProfile.Country;
                userProfile.Telegram = updatedUserProfile.Telegram;
                userProfile.Skype = updatedUserProfile.Skype;
                userProfile.Twitter = updatedUserProfile.Twitter;
                userProfile.Instagram = updatedUserProfile.Instagram;
                userProfile.Facebook = updatedUserProfile.Facebook;
                userProfile.Snapchat = updatedUserProfile.Snapchat;
                userProfile.Youtube = updatedUserProfile.Youtube;
                userProfile.Blog = updatedUserProfile.Blog;
                userProfile.DateOfBirth = updatedUserProfile.DateOfBirth;

                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error updating user profile", updatedUserProfile.UserId);
                throw;
            }

            return Unit.Value;
        }
    }
}
