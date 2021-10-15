using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using Refit;

namespace CriThink.Client.Core.Api
{
    public interface INewsSourceApi
    {
        [Get("/" + EndpointConstants.NewsSourceAllQuestions)]
        Task<NewsSourceGetAllQuestionsResponse> GetNewsSourceQuestionsAsync(
            [Header("Accept-Language")]string Language,
            CancellationToken cancellationToken = default);

        [Post("/" + EndpointConstants.NewsSourcePostAnswerToQuestions)]
        Task<NewsSourcePostAnswersResponse> PostAnswersToArticleQuestionsAsync(
            [Body] NewsSourcePostAllAnswersRequest request,
            CancellationToken cancellationToken = default);
    }
}