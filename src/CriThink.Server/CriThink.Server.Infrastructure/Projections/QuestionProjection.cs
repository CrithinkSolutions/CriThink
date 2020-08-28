using System;
using System.Linq.Expressions;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Infrastructure.Projections
{
    internal static class QuestionProjection
    {
        /// <summary>
        /// Get all the items and converts them to a list of <see cref="Question"/>
        /// </summary>
        internal static Expression<Func<Question, Question>> GetAll =>
            question => new Question
            {
                Id = question.Id,
                Content = question.Content
            };
    }
}
