﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

#pragma warning disable CA2227 // Collection properties should be read only

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.NewsAnalyzer
{
    public class QuestionAnswerRequest : IValidatableObject
    {
        [Required]
        [JsonPropertyName("newsId")]
        public Guid NewsId { get; set; }

        [JsonPropertyName("answers")]
        [Required]
        public IList<AnswerRequest> Answers { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NewsId.Equals(Guid.Empty))
            {
                yield return new ValidationResult("You can't use a default GUID as Id", new[] { nameof(NewsId) });
            }
        }
    }

    public class AnswerRequest : IValidatableObject
    {
        [Required]
        [JsonPropertyName("questionId")]
        public Guid QuestionId { get; set; }

        [Required]
        [JsonPropertyName("isPositive")]
        public bool IsPositive { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (QuestionId.Equals(Guid.Empty))
            {
                yield return new ValidationResult("You can't use a default GUID as Id", new[] { nameof(QuestionId) });
            }
        }
    }
}
