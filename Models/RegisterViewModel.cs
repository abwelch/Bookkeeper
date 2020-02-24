using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Bookkeeper.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "Email cannot exceed {1} characters.")]
        public string Email { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Username cannot exceed {1} characters.")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password must be at least {2} characters.", MinimumLength = 6 )]

        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare ("Password", ErrorMessage = "Password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
