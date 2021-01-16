using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.IdentityProvider
{
    public class EmailConfirmationRequest : IValidatableObject
    {
        [JsonPropertyName("userId")]
        [Required]
        public Guid UserId { get; set; }

        [JsonPropertyName("code")]
        [Required]
        public string Code { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserId == Guid.Empty)
            {
                yield return new ValidationResult("Guid can't be empty", new[] { nameof(UserId) });
            }
        }
    }
}
