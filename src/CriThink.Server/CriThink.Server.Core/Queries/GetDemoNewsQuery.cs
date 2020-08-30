using System;
using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetDemoNewsQuery : IRequest<DemoNews>
    {
        public GetDemoNewsQuery(Guid demoNewsId)
        {
            DemoNewsId = demoNewsId;
        }

        public Guid DemoNewsId { get; }
    }
}
