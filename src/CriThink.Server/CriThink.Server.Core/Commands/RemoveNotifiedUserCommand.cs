using System;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class RemoveNotifiedUserCommand : IRequest
    {
        public RemoveNotifiedUserCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}