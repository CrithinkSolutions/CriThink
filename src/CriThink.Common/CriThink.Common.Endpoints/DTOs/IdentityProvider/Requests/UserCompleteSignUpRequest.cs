using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
// ReSharper disable CheckNamespace

namespace CriThink.Common.Endpoints.DTOs.IdentityProvider
{
    public class UserCompleteSignUpRequest
    {
        [JsonPropertyName("userId")]
        [Required]
        public Guid UserId { get; set; }
    }
}
