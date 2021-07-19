using System;
using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.Article
{
    public class ArticleGetQuestionResponse
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }
}
