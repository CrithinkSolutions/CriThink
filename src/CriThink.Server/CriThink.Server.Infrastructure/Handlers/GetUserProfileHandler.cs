using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Queries;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    internal class GetUserProfileHandler : IRequestHandler<GetUserProfileQuery, UserProfile>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<GetUserProfileHandler> _logger;

        public GetUserProfileHandler(CriThinkDbContext dbContext, ILogger<GetUserProfileHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<UserProfile> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var userProfile = await _dbContext.UserProfiles
                    .Include(p => p.User)
                    .SingleOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken)
                    .ConfigureAwait(false);

                return userProfile;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "ERror getting user profile details", request);
                throw;
            }
        }
    }
}
