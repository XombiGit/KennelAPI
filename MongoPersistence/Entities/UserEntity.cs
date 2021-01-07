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
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

        public IUserEntity Clone()
        {
            var copy = (UserEntity)this.MemberwiseClone();
            return copy;
        }
    }
}
