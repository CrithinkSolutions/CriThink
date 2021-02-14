using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetDebunkingNewsPublisherByNameQuery : IRequest<DebunkingNewsPublisher>
    {
        public GetDebunkingNewsPublisherByNameQuery(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
