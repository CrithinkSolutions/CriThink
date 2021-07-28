using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Application.Services
{
    internal interface IUserAvatarService
    {
        Task UpdateUserProfileAvatarAsync(Guid userId, IFormFile formFile, CancellationToken cancellationToken = default);

        Task UpdateUserProfileAvatarAsync(Guid userId, byte[] bytes, CancellationToken cancellationToken = default);
    }
}
