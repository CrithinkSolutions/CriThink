using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceSearchRequest
    {
        [JsonPropertyName("newsLink")]
        [Required]
        [MinLength(2)]
        public string NewsLink { get; set; }
    }
}
