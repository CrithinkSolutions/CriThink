using System.Collections.Generic;
using CriThink.Server.Core.Responses;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetAllDemoNewsQuery : IRequest<List<GetAllDemoNewsQueryResponse>>, IRequest<Unit>
    { }
}
