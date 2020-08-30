using System.Collections.Generic;
using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetAllDemoNewsQuery : IRequest<List<DemoNews>>
    { }
}
