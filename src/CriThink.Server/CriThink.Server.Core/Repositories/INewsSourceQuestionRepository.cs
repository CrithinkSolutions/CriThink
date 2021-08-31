using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Core.Repositories
{
    public interface INewsSourceQuestionRepository
    {
        Task<IList<ArticleQuestion>> GetQuestionsByCategoryAsync(
            QuestionCategory category,
            CancellationToken cancellationToken = default);
    }
}
