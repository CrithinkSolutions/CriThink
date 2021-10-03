using System;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    public class UpdateUserRoleCommand : IRequest
    {
        public UpdateUserRoleCommand(Guid id, string role)
        {
            Id = id;
            Role = role;
        }

        public Guid Id { get; }

        public string Role { get; }
    }
}
