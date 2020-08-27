using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.NewsAnalyzer
{
    public class QuestionAnswerAddRequest : IValidatableObject
    {
        [JsonPropertyName("newsId")]
        [Required]
        public Guid NewsId { get; set; }

        [JsonPropertyName("questionId")]
        [Required]
        public Guid QuestionId { get; set; }

        [JsonPropertyName("isPositive")]
        [Required]
        public bool IsPositive { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NewsId.Equals(Guid.Empty))
            {
                yield return new ValidationResult("You can't use a default GUID as Id", new[] { nameof(NewsId) });
            }

            if (QuestionId.Equals(Guid.Empty))
            {
                yield return new ValidationResult("You can't use a default GUID as Id", new[] { nameof(QuestionId) });
            }
        }
    }
}
