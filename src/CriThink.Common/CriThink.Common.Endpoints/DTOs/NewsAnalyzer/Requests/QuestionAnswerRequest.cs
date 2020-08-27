using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

#pragma warning disable CA2227 // Collection properties should be read only

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.NewsAnalyzer
{
    public class QuestionAnswerRequest
    {
        [Required]
        [JsonPropertyName("newsId")]
        public string NewsId { get; set; }

        [JsonPropertyName("answers")]
        [Required]
        public IList<AnswerRequest> Answers { get; set; }
    }

    public class AnswerRequest
    {
        [Required]
        [JsonPropertyName("questionId")]
        public string QuestionId { get; set; }

        [Required]
        [JsonPropertyName("isPositive")]
        public bool IsPositive { get; set; }
    }
}
