using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KennelAPI.Services
{
    public class InMemoryDogEntity : IDogEntity
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
        public string OwnerID { get; set; }

        public IDogEntity Clone()
        {
            var copy = (InMemoryDogEntity)this.MemberwiseClone();
            return copy;
        }
    }
}
