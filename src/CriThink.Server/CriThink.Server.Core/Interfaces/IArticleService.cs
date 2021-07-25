using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.Article;

namespace CriThink.Server.Core.Interfaces
{
    public interface IArticleService
    {
        /// <summary>
        /// Returns questions for an article
        /// </summary>
        /// <returns></returns>
        Task<ArticleGetAllQuestionsResponse> GetQuestionsAsync();

        /// <summary>
        /// Gives answers to questions for the specified article
        /// </summary>
        /// <param name="request">Answers</param>
        /// <returns></returns>
        Task<ArticlePostAnswersResponse> PostAnswersToArticleQuestionsAsync(ArticlePostAllAnswersRequest request);
    }
}
