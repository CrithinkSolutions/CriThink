using System;
using System.Collections.Generic;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Core.Responses
{
    public class GetAllArticleQuestionsResponse
    {
        public GetAllArticleQuestionsResponse(IEnumerable<ArticleQuestion> questions)
        {
            if (questions is null)
                throw new ArgumentNullException(nameof(questions));

            Questions = new List<ArticleQuestion>(questions);
        }

        public IList<ArticleQuestion> Questions { get; }
    }
}
