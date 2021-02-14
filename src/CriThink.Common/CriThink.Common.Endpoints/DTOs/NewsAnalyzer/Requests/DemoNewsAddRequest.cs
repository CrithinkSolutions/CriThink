using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.NewsAnalyzer
{
    public class DemoNewsAddRequest
    {
        [Required]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [Url]
        [Required]
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
}
