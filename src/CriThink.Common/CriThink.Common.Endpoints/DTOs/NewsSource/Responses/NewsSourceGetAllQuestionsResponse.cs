using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceGetAllQuestionsResponse
    {
        [JsonPropertyName("questions")]
        public IList<NewsSourceGetQuestionResponse> Questions { get; set; }
    }
}
