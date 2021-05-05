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

                if (updatedUserProfile.FamilyName is not null)
                    userProfile.FamilyName = updatedUserProfile.FamilyName;

                if (updatedUserProfile.GivenName is not null)
                    userProfile.GivenName = updatedUserProfile.GivenName;

                if (updatedUserProfile.Description is not null)
                    userProfile.Description = updatedUserProfile.Description;

                if (updatedUserProfile.Gender is not null)
                    userProfile.Gender = updatedUserProfile.Gender;

                if (updatedUserProfile.Country is not null)
                    userProfile.Country = updatedUserProfile.Country;

                if (updatedUserProfile.Telegram is not null)
                    userProfile.Telegram = updatedUserProfile.Telegram;

                if (updatedUserProfile.Skype is not null)
                    userProfile.Skype = updatedUserProfile.Skype;

                if (updatedUserProfile.Twitter is not null)
                    userProfile.Twitter = updatedUserProfile.Twitter;

                if (updatedUserProfile.Instagram is not null)
                    userProfile.Instagram = updatedUserProfile.Instagram;

                if (updatedUserProfile.Facebook is not null)
                    userProfile.Facebook = updatedUserProfile.Facebook;

                if (updatedUserProfile.Snapchat is not null)
                    userProfile.Snapchat = updatedUserProfile.Snapchat;

                if (updatedUserProfile.Youtube is not null)
                    userProfile.Youtube = updatedUserProfile.Youtube;

                if (updatedUserProfile.Blog is not null)
                    userProfile.Blog = updatedUserProfile.Blog;

                if (updatedUserProfile.DateOfBirth is not null)
                    userProfile.DateOfBirth = updatedUserProfile.DateOfBirth;

                _dbContext.UserProfiles.Update(userProfile);

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
