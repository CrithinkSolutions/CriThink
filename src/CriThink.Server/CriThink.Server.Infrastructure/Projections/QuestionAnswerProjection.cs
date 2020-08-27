using System;
using System.Linq.Expressions;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Infrastructure.Projections
{
    internal static class QuestionAnswerProjection
    {
        /// <summary>
        /// Get all the items and converts them to a list of <see cref="Question"/>
        /// </summary>
        internal static Expression<Func<QuestionAnswer, QuestionAnswer>> GetAll =>
            questionAnswer => new QuestionAnswer
            {
                IsTrue = questionAnswer.IsTrue,
                Question = questionAnswer.Question,
                //Id = questionAnswer.Id
            };
    }
}
