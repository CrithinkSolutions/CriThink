using System;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    public class DeleteDebunkingNewsCommand : IRequest
    {
        public DeleteDebunkingNewsCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
