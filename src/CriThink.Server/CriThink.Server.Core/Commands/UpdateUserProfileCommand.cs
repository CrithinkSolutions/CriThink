using System;
using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class UpdateUserProfileCommand : IRequest
    {
        public UpdateUserProfileCommand(UserProfile userProfile)
        {
            UserProfile = userProfile ?? throw new ArgumentNullException(nameof(userProfile));
        }

        public UserProfile UserProfile { get; }
    }
}
