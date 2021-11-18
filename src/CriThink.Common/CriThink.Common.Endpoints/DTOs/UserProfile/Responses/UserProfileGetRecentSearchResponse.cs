using System;
using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.UserProfile
{
    public class UserProfileGetRecentSearchResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("searchedText")]
        public string SearchedText { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonPropertyName("favicon")]
        public string Favicon { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}
