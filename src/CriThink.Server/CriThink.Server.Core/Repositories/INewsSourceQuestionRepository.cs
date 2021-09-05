using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Core.Repositories
{
    public interface INewsSourceQuestionRepository : IRepository<ArticleQuestion>
    {
        Task<IList<ArticleQuestion>> GetQuestionsByCategoryAsync(
            QuestionCategory category,
            CancellationToken cancellationToken = default);
    }
}
