using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CriThink.Common.Endpoints.Converters;

// Resharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceAddRequest
    {
        [MinLength(2)]
        [Required]
        [JsonPropertyName("newsLink")]
        public string NewsLink { get; set; }

        [Required]
        [JsonPropertyName("classification")]
        [JsonConverter(typeof(NewsSourceClassificationConverter))]
        public NewsSourceClassification Classification { get; set; }
    }
}
