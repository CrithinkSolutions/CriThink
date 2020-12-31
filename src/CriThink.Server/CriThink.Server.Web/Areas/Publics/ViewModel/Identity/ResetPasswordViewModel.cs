﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Web.Areas.Publics.ViewModel.Identity
{
    public class ResetPasswordViewModel : IValidatableObject
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string ConfirmPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password != ConfirmPassword)
                yield return new ValidationResult("Password must essere la stessa", new[] { nameof(Password), nameof(ConfirmPassword) });
        }
    }
}
