using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
// ReSharper disable CheckNamespace

namespace CriThink.Common.Endpoints.DTOs.IdentityProvider
{
    public class ChangePasswordRequest
    {
        [JsonPropertyName("currentPassword")]
        [Required]
        public string CurrentPassword { get; set; }

        [JsonPropertyName("newPassword")]
        [Required]
        public string NewPassword { get; set; }
    }
}
