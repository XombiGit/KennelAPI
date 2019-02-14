using Common.Entities;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoPersistence.Entities
{
    public class UserEntity : IUserEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id { get; set; }

        public string UserID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public IUserEntity Clone()
        {
            var copy = (UserEntity)this.MemberwiseClone();
            return copy;
        }
    }
}
