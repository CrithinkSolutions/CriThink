using System;
using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetDebunkingNewsPublisherByIdQuery : IRequest<DebunkingNewsPublisher>
    {
        public GetDebunkingNewsPublisherByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
