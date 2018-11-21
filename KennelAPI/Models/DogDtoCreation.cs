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
        public string Name { get; set; }
        public int DogID { get; set; }
        public string Breed { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int XCoord { get; set; }
        public int YCoord { get; set; }
        public string SpecialNotes { get; set; }
        public int Reward { get; set; }
        public string ImageURL { get; set; }
    }
}
