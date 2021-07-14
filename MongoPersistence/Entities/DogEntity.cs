using Common.Entities;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Common.Models;

namespace MongoPersistence.Entities
{
    public class DogEntity : IDogEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id { get; set; }
        //difference between 2 ids ?
        public string DogID { get; set; }
        //public string Name { get; set; }
        //public string Breed { get; set; }
        //public string Phone { get; set; }
        //public string Email { get; set; }
        public int XCoord { get; set; }
        public int YCoord { get; set; }
        //public string SpecialNotes { get; set; }
        //public int Reward { get; set; }
        public string ImageURL { get; set; }
        public string OwnerID { get; set; }
        public DogStatus.Status dogStatus { get; set; }
        public bool withOwner { get; set; }
        public IDogEntity Clone()
        {
            var copy = (DogEntity)this.MemberwiseClone();
            return copy;
        }
    }
}
