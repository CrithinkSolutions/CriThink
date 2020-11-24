using System;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class DeleteDebunkingNewsCommand : IRequest
    {
        public DeleteDebunkingNewsCommand(Guid debunkingNewsId)
        {
            DebunkingNewsId = debunkingNewsId;
        }

        public Guid DebunkingNewsId { get; }
    }
}
