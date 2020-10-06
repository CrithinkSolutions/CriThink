using System;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetLastDebunkinNewsFetchTimeQuery : IRequest<DateTime>
    { }
}
