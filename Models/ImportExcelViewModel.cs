using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Bookkeeper.Models
{
    public class ImportExcel
    {
        [Required(ErrorMessage = "Please select a file")]
        [FileExtensionValidation(allow = ".xls, .xlsx", ErrorMessage = "Only Excel files are accepted (.xls or .xlsx)")]
        public IFormFile ExcelFile { get; set; }

        public bool ImportSuccess { get; set; }
    }
}
