using Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MongoPersistence.Entities
{
    public class DogEntity : IDogEntity
    {
        public string DogID { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int XCoord { get; set; }
        public int YCoord { get; set; }
        public string SpecialNotes { get; set; }
        public int Reward { get; set; }
        public string ImageURL { get; set; }
        public int OwnerID { get; set; }

        public IDogEntity Clone()
        {
            var copy = (DogEntity)this.MemberwiseClone();
            return copy;
        }
    }
}
