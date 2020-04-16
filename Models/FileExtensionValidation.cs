using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Bookkeeper.Models
{
    public class FileExtensionValidation : ValidationAttribute
    {
        public string allow;
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string extension = ((IFormFile)value).FileName.Split('.')[1];
                if (allow.Contains(extension))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
