using System.Text.Json.Serialization;

// ReSharper disable CheckNamespace
#pragma warning disable CA1056 // Uri properties should not be strings

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceGetAllResponse
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("classification")]
        public string NewsSourceClassification { get; set; }
    }
}
