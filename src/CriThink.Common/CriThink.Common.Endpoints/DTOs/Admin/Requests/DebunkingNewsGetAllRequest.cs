using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class DebunkingNewsGetAllRequest : IValidatableObject, IQueryStringRequest
    {
        [Required]
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [Required]
        [JsonPropertyName("pageIndex")]
        public int PageIndex { get; set; }

        [JsonPropertyName("languageFilter")]
        public DebunkingNewsGetAllLanguageFilterRequests LanguageFilters { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PageSize < 1)
                yield return new ValidationResult("PageSize can't be less than 1");

            if (PageIndex < 0)
                yield return new ValidationResult("PageIndex can't be negative");
        }

        public string ToQueryString() => $"{nameof(PageSize)}={PageSize}&{nameof(PageIndex)}={PageIndex}&{nameof(LanguageFilters)}={LanguageFilters}";
    }
}
