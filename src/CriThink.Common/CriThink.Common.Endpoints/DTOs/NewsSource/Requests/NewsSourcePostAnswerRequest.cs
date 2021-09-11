using System;
using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourcePostAnswerRequest
    {
        [JsonPropertyName("questionId")]
        public Guid QuestionId { get; set; }

        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}
