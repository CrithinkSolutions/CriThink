using System;
using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.IdentityProvider
{
    public class UserSoftDeletionResponse
    {
        [JsonPropertyName("deletionScheduledOn")]
        public DateTimeOffset DeletionScheduledOn { get; set; }
    }
}
