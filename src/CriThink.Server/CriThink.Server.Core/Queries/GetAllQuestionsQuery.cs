using System.Collections.Generic;
using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetAllQuestionsQuery : IRequest<List<Question>>, IRequest<Unit>
    {
    }
}
