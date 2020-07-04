using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
// ReSharper disable CheckNamespace

namespace CriThink.Web.Models.DTOs.IdentityProvider
{
    public class UserLoginRequest : IValidatableObject
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(UserName))
            {
                yield return new ValidationResult("Email or UserName must have a valid value", new[] { nameof(Email), nameof(UserName) });
            }
        }
    }
}
