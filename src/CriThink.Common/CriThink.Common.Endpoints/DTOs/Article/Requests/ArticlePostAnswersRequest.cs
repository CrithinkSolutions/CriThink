using System;
using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.Article
{
    public class ArticlePostAnswersRequest
    {
        [JsonPropertyName("questionId")]
        public Guid QuestionId { get; set; }

        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}
