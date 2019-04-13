using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public interface IDogEntity
    {
        string DogID { get; set; }
        string Name { get; set; }
        string Breed { get; set; }
        string Phone { get; set; }
        string Email { get; set; }
        int XCoord { get; set; }
        int YCoord { get; set; }
        string SpecialNotes { get; set; }
        int Reward { get; set; }
        string ImageURL { get; set; }
        string OwnerID { get; set; }

        IDogEntity Clone();
    }
}
