using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class UserRoleUpdateRequest : IValidatableObject
    {
        [JsonPropertyName("userId")]
        [Required]
        public Guid UserId { get; set; }

        [JsonPropertyName("role")]
        [Required]
        public string Role { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserId == Guid.Empty)
            {
                yield return new ValidationResult("Guid can't be empty", new[] { nameof(UserId) });
            }
        }
    }
}
