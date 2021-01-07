using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KennelAPI.Models
{
    public class DogDtoUpdate
    {
        [MaxLength(10, ErrorMessage = "Name can only be 10 characters")]
        public string Name { get; set; }
        public int DogID { get; set; }
        public string Breed { get; set; }

        [MaxLength(10, ErrorMessage = "Phone # can only be 10 digits")]
        public string Phone { get; set; }
        [MaxLength(25, ErrorMessage = "Email can only be 25 characters")]
        public string Email { get; set; }

        //TODO: Fix min an max
        public int XCoord { get; set; }
        public int YCoord { get; set; }
        public string SpecialNotes { get; set; }
        public int Reward { get; set; }
        public string ImageURL { get; set; }
    }
}
