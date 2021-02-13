using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Web;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Common
{
    public class SimpleUriRequest : IQueryStringRequest
    {
        [Url]
        [Required]
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        public string ToQueryString() => $"{nameof(Uri)}={HttpUtility.UrlEncode(Uri)}";
    }
}
