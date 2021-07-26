using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Core.Repositories
{
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        Task<UserProfile> GetUserProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        UserProfile SaveUserProfileAsync(UserProfile userProfile);
    }
}
