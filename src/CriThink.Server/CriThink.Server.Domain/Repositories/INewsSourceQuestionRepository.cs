using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;

namespace CriThink.Server.Domain.Repositories
{
    public interface INewsSourceQuestionRepository
    {
        Task<IList<NewsSourcePostQuestion>> GetQuestionsByCategoryAsync(
            QuestionCategory category,
            CancellationToken cancellationToken = default);
    }
}
