using System;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class UnknownNewsSourceGetResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("requestedAt")]
        public string RequestedAt { get; set; }

        [JsonPropertyName("requestCount")]
        public int RequestCount { get; set; }

        [JsonPropertyName("authenticity")]
        public string Authenticity { get; set; }

        [JsonPropertyName("identifiedAt")]
        public string IdentifiedAt { get; set; }
    }
}