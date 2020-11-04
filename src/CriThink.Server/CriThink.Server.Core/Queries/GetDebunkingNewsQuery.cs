using System;
using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetDebunkingNewsQuery : IRequest<DebunkingNews>
    {
        public GetDebunkingNewsQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
