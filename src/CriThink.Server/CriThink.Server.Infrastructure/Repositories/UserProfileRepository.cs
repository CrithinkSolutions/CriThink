using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Repositories;
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

        public IUnitOfWork UnitOfWork => _context;

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

        public UserProfile SaveUserProfileAsync(
            UserProfile userProfile,
            CancellationToken cancellationToken = default)
        {
            return _context.UserProfiles.Update(userProfile).Entity;
        }
    }
}
