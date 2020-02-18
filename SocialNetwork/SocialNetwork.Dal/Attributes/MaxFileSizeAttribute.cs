using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SocialNetwork.Dal.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            if (!(value is IFormFile file)) return ValidationResult.Success;

            return file.Length > _maxFileSize ? new ValidationResult(GetErrorMessage()) : ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Maximum allowed file size is { _maxFileSize} bytes.";
        }
    }
}
