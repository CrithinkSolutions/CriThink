using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class DebunkingNewsUpdateRequest : IValidatableObject
    {
        [JsonPropertyName("newsId")]
        [Required]
        public Guid Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("caption")]
        public string Caption { get; set; }

        [Url]
        [JsonPropertyName("uri")]
        public string Link { get; set; }

        [JsonPropertyName("keywords")]
        public IReadOnlyList<string> Keywords { get; set; }

        [JsonPropertyName("imageLink")]
        public string ImageLink { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id == Guid.Empty)
            {
                yield return new ValidationResult("Guid can't be empty", new[] { nameof(Id) });
            }
        }
    }
}
