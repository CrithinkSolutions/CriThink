using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Web;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceSearchRequest : IQueryStringRequest
    {
        [JsonPropertyName("newsLink")]
        [Required]
        [MinLength(2)]
        public string NewsLink { get; set; }

        public string ToQueryString() => $"{nameof(NewsLink)}={HttpUtility.UrlEncode(NewsLink)}";
    }
}
