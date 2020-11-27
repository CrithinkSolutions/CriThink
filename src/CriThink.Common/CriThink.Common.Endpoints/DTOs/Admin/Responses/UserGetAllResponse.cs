using System.Collections.Generic;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class UserGetAllResponse
    {
        public UserGetAllResponse(IEnumerable<UserGetResponse> users, bool hasNextPage)
        {
            Users = new List<UserGetResponse>(users);
            HasNextPage = hasNextPage;
        }

        [JsonPropertyName("users")]
        public IReadOnlyList<UserGetResponse> Users { get; }

        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; }
    }
}
