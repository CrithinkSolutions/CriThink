using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.NewsAnalyzer
{
    public class QuestionAnswerAddRequest : IValidatableObject
    {
        [Required]
        [JsonPropertyName("newsId")]
        public Guid NewsId { get; set; }

        [Required]
        [JsonPropertyName("questionId")]
        public Guid QuestionId { get; set; }

        [Required]
        [JsonPropertyName("isPositive")]
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
