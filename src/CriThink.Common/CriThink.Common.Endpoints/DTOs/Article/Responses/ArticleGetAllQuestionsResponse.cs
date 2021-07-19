using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.Article
{
    public class ArticleGetAllQuestionsResponse
    {
        [JsonPropertyName("questions")]
        public IList<ArticleGetQuestionResponse> Questions { get; set; }
    }
}
