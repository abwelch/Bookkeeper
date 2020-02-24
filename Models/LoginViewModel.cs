using System.ComponentModel.DataAnnotations;

namespace Bookkeeper.Models
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(30, ErrorMessage = "Username cannot exceed {1} characters.")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
        public string Password { get; set; }
    }
}
