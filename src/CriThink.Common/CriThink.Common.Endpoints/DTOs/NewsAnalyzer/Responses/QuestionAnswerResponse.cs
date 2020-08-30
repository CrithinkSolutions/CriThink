using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.NewsAnalyzer
{
    public class QuestionAnswerResponse
    {
        [Required]
        [JsonPropertyName("questionId")]
        public string QuestionId { get; set; }

        [Required]
        [JsonPropertyName("isCorrect")]
        public bool IsCorrect { get; set; }
    }
}
