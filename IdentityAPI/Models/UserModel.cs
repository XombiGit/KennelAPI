using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAPI.Models
{
    public class UserModel
    {
        [Required]
        [Display(Name = "User name")]
        [StringLength(8, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
       // public string ConfirmPassword { get; set; }

        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
