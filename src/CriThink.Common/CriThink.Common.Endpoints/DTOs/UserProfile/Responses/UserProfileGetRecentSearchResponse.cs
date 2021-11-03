using System;
using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.UserProfile
{
    public class UserProfileGetRecentSearchResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("newsLink")]
        public string NewsLink { get; set; }
    }
}
