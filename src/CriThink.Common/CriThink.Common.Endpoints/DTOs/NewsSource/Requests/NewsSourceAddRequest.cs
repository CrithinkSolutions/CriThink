using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CriThink.Common.Endpoints.Converters;

// Resharper disable once CheckNamespace
#pragma warning disable CA1056 // Uri properties should not be strings

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceAddRequest
    {
        [Url]
        [Required]
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [Required]
        [JsonPropertyName("classification")]
        [JsonConverter(typeof(NewsSourceClassificationConverter))]
        public NewsSourceClassification Classification { get; set; }
    }
}
