using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KennelAPI.Models
{
    public class UserDtoCreation
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone # is required")]
        [MaxLength(10)]
        public string Phone { get; set; }

        public string Email { get; set; }
    }
}
