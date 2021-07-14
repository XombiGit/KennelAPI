using Common.Entities;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoPersistence.Entities
{
    public class LocationEntity : ILocationEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id { get; set; }//row id
        public string DogID { get; set; }
        public float XCoord { get; set; }
        public float YCoord { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
