using System;
using System.Collections.Generic;
using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetAllQuestionAnswerQuery : IRequest<List<QuestionAnswer>>
    {
        public GetAllQuestionAnswerQuery(Guid newsId, IEnumerable<Guid> filters)
        {
            if (filters == null)
                throw new ArgumentNullException(nameof(filters));

            NewsId = newsId;
            QuestionIds = new List<Guid>(filters).AsReadOnly();
        }

        public IReadOnlyList<Guid> QuestionIds { get; }

        public Guid NewsId { get; }
    }
}
