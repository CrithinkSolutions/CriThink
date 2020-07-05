using System.Text.Json.Serialization;

namespace CriThink.Server.Web.Models.DTOs.NewsAnalyzer.Responses
{
    public class HttpsSupportResponse
    {
        [JsonPropertyName("isSecure")]
        public bool IsSecure { get; set; }
    }
}
