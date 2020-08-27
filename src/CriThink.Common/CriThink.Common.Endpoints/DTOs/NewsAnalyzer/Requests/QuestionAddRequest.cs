using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.NewsAnalyzer
{
    public class QuestionAddRequest
    {
        [Required]
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
