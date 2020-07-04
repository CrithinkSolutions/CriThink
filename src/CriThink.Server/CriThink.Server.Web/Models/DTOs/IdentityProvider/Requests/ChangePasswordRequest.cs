using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
// ReSharper disable CheckNamespace

namespace CriThink.Web.Models.DTOs.IdentityProvider
{
    public class ChangePasswordRequest
    {
        [JsonProperty("currentPassword")]
        [Required]
        public string CurrentPassword { get; set; }

        [JsonProperty("newPassword")]
        [Required]
        public string NewPassword { get; set; }
    }
}
