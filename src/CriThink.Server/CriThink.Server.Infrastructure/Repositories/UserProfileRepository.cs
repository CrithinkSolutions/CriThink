using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.Repositories
{
    internal class UserProfileRepository : IUserProfileRepository
    {
        private readonly CriThinkDbContext _context;

        public UserProfileRepository(CriThinkDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfile> GetUserProfileByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var userProfile = await _context.UserProfiles
                    .Include(p => p.User)
                    .SingleOrDefaultAsync(u => u.UserId == userId, cancellationToken)
                    .ConfigureAwait(false);

            return userProfile;
        }
    }
}
