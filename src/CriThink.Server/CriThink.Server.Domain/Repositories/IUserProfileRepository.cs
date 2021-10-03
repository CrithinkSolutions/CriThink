using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;

namespace CriThink.Server.Domain.Repositories
{
    public interface IUserProfileRepository
    {
        Task<UserProfile> GetUserProfileByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
