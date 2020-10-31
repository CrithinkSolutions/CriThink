using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class UserGetResponse
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("userEmail")]
        public string UserEmail { get; set; }

        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("isEmailConfirmed")]
        public bool IsEmailConfirmed { get; set; }

        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonPropertyName("accessFailedCount")]
        public int AccessFailedCount { get; set; }

        [JsonPropertyName("isLockoutEnabled")]
        public bool IsLockoutEnabled { get; set; }

        [JsonPropertyName("lockoutEnd")]
        public DateTimeOffset? LockoutEnd { get; set; }

        [JsonPropertyName("role")]
        #pragma warning disable CA2227
        public IReadOnlyCollection<string> Roles { get; set; }
    }
}
