using System;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class UpdateUserScheduledDeletionCommand : IRequest
    {
        public UpdateUserScheduledDeletionCommand(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}
