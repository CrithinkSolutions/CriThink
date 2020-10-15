using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class UserUpdateRequest : IValidatableObject
    {
        [JsonPropertyName("userId")]
        [Required]
        public Guid UserId { get; set; }

        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("isEmailConfirmed")]
        public bool? IsEmailConfirmed { get; set; }

        [JsonPropertyName("accessFailedCount")]
        public int? AccessFailedCount { get; set; }

        [JsonPropertyName("isLockoutEnabled")]
        public bool? IsLockoutEnabled { get; set; }

        [JsonPropertyName("lockoutEnd")]
        public DateTime? LockoutEnd { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserId == Guid.Empty)
            {
                yield return new ValidationResult("Guid can't be empty", new[] { nameof(UserId) });
            }
        }
    }
}
