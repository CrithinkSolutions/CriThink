using System;
using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class DeleteUserLogicallyCommand : IRequest<User>
    {
        public DeleteUserLogicallyCommand(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            UserId = Guid.Parse(userId);
        }

        public Guid UserId { get; }
    }
}
