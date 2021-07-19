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
    }
}
