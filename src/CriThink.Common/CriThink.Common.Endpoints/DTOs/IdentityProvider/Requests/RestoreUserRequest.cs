using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.IdentityProvider
{
    public class RestoreUserRequest : IValidatableObject
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(Username))
            {
                yield return new ValidationResult("Email or UserName must have a valid value", new[] { nameof(Email), nameof(Username) });
            }
        }
    }
}
