using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.UserProfile
{
    public class UserProfileGetAllRecentSearchResponse
    {
        [JsonPropertyName("recentSearches")]
        public ICollection<UserProfileGetRecentSearchResponse> RecentSearches { get; set; }
    }
}
