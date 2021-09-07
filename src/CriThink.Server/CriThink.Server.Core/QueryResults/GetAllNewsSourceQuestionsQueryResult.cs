using System;
using System.Collections.Generic;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Core.QueryResults
{
    public class GetAllNewsSourceQuestionsQueryResult
    {
        public GetAllNewsSourceQuestionsQueryResult(IEnumerable<NewsSourcePostQuestion> questions)
        {
            if (questions is null)
                throw new ArgumentNullException(nameof(questions));

            Questions = new List<NewsSourcePostQuestion>(questions);
        }

        public IList<NewsSourcePostQuestion> Questions { get; }
    }
}
