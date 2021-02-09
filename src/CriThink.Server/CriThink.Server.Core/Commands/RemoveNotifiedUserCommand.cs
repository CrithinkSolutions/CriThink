using System;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    class RemoveNotifiedUserCommand : IRequest
    {
        public RemoveNotifiedUserCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}