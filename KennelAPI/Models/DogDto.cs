using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;

namespace KennelAPI.Models
{
    public class DogDto
    {
        //public string Name { get; set; }
        public string DogID { get; set; }
        //public string Breed { get; set; }
        //public string Phone { get; set; }
        //public string Email { get; set; }
        public int XCoord { get; set; }
        public int YCoord { get; set; }
        //public string SpecialNotes { get; set; }
        //public int Reward { get; set; }
        public string ImageURL { get; set; }
        public DogStatus.Status dogStatus { get; set; }
        public string statusDescription { get; set; }
        public bool withOwner { get; set; }
    }
}

