using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
// ReSharper disable CheckNamespace

namespace CriThink.Web.Models.DTOs.IdentityProvider
{
    public class ResetPasswordRequest
    {
        [JsonPropertyName("userId")]
        [Required]
        public string UserId { get; set; }

        [JsonPropertyName("token")]
        [Required]
        public string Token { get; set; }

        [JsonPropertyName("newPassword")]
        [Required]
        public string NewPassword { get; set; }
    }
}
