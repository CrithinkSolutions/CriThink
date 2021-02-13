﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class DebunkingNewsGetRequest : IValidatableObject, IQueryStringRequest
    {
        [JsonPropertyName("id")]
        [Required]
        public Guid Id { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id == Guid.Empty)
            {
                yield return new ValidationResult("Guid can't be empty", new[] { nameof(Id) });
            }
        }

        public string ToQueryString() => $"{nameof(Id)}={Id.ToString().ToLowerInvariant()}";
    }
}
