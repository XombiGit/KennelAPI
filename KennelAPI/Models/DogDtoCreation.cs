using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KennelAPI.Models
{
    public class DogDtoCreation
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(10, ErrorMessage = "Name can only be 10 characters")]
        public string Name { get; set; }

        public string Breed { get; set; }

        [Required(ErrorMessage = "Phone # is required")]
        [MaxLength(10, ErrorMessage = "Phone # can only be 10 digits")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [MaxLength(25, ErrorMessage = "Email can only be 25 characters")]
        public string Email { get; set; }

        public int XCoord { get; set; }

        public int YCoord { get; set; }

        public string SpecialNotes { get; set; }

        public int Reward { get; set; }

        [Required(ErrorMessage = "ImageURL is required")]
        public string ImageURL { get; set; }

        [Required(ErrorMessage = "OwnerID is required")]
        public string OwnerID { get; set; }
    }
}
