using System;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    /// <summary>
    /// Logically deletes the given user
    /// </summary>
    public class DeleteUserCommand : IRequest
    {
        public DeleteUserCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
