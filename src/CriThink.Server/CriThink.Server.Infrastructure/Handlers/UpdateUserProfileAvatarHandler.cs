using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    internal class UpdateUserProfileAvatarHandler : IRequestHandler<UpdateUserProfileAvatarCommand>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<UpdateUserProfileAvatarHandler> _logger;

        public UpdateUserProfileAvatarHandler(CriThinkDbContext dbContext, ILogger<UpdateUserProfileAvatarHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserProfileAvatarCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var userId = Guid.Parse(request.UserId);

                var userProfile = await _dbContext.UserProfiles.FirstOrDefaultAsync(up => up.UserId == userId, cancellationToken);
                if (userProfile is null)
                    throw new ResourceNotFoundException("User not found");

                userProfile.AvatarPath = request.Path;

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error updating user profile avatar", request.UserId);
                throw;
            }

            return Unit.Value;
        }
    }
}
