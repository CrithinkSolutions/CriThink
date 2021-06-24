using System;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class UpdateUserProfileAvatarCommand : IRequest
    {
        public UpdateUserProfileAvatarCommand(string userId, string path)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            UserId = userId;
            Path = path;
        }

        public string UserId { get; }

        public string Path { get; }
    }
}