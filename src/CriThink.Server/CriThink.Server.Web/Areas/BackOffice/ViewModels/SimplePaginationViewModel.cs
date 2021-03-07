using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels
{
    public class SimplePaginationViewModel : IValidatableObject
    {
        public int PageSize { get; set; } = 20;

        public int PageIndex { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PageSize < 1)
            {
                yield return new ValidationResult("Page size cannot be less than 1", new[] { nameof(PageSize) });
            }

            if (PageIndex < 0)
            {
                yield return new ValidationResult("Page index cannot be less than 1", new[] { nameof(PageIndex) });
            }
        }
    }
}