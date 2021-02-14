using System;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class NotificationRequestGetResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("requestedAt")]
        public string RequestedAt { get; set; }

        [JsonPropertyName("requestedLink")]
        public string RequestedLink { get; set; }

        [JsonPropertyName("requestCount")]
        public int RequestCount { get; set; }
    }
}
