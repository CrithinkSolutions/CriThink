using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Web.ActionFilters
{
    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly int _minFileSize;
        private readonly int _maxFileSize;

        public FileSizeAttribute(int minFileSize, int maxFileSize)
        {
            _minFileSize = minFileSize;
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not IFormFile file)
            {
                return new ValidationResult("Invalid file");
            }

            if (file.Length < _minFileSize)
                return new ValidationResult($"Minimum allowed file size is {_minFileSize} bytes.");

            if (file.Length > _maxFileSize)
            {
                return new ValidationResult($"Maximum allowed file size is {_maxFileSize} bytes.");
            }

            return ValidationResult.Success;
        }
    }
}
