using System;
using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetQuestionQuery : IRequest<Question>
    {
        public GetQuestionQuery(Guid questionId)
        {
            QuestionId = questionId;
        }

        public Guid QuestionId { get; }
    }
}
