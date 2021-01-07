using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;
using Common.Interfaces;
using MongoDB.Driver;
using MongoPersistence.Entities;

namespace MongoPersistence.Services
{
    public class UserRepository : IUserRepository
    {
        MongoConnectionManager _connection;
        IMongoCollection<UserEntity> _userCollection;

        public UserRepository()
        {
            _connection = new MongoConnectionManager();
            _userCollection = _connection.GetCollection<UserEntity>("UserDomain");
        }

        public async Task AddUser(IUserEntity userEntity)
        {
            UserEntity user = (UserEntity)userEntity;
            await _userCollection.InsertOneAsync(user);
        }

        public async void DeleteUser(IUserEntity userEntity)
        {
            UserEntity userToDelete = (UserEntity)userEntity;
            string userId = userToDelete.UserID;
            await _userCollection.DeleteOneAsync<UserEntity>(p => p.UserID == userId);
        }

        public async Task<IUserEntity> GetUser(string userId)
        {
            //IMongoCollection<DogEntity> dogEntity = (IMongoCollection<DogEntity>)_dogCollection;
            var result = await _userCollection.Find<UserEntity>(p => p.UserID == userId).FirstOrDefaultAsync();
            return result;
        }

        public async void UpdateUser(IUserEntity userToUpdate)
        {
            //var existingDog = await GetDog(dogToUpdate.DogID);
            UserEntity existingUser = (UserEntity)userToUpdate;
            string userId = existingUser.UserID;
            //string name = existingUser.Name;

            var filter = Builders<UserEntity>.Filter.Eq(m => m.UserID, userId);

            if (userToUpdate != null)
            {
                //var update = Builders<DogEntity>.Update.Set(m => m, existingDog);
                await _userCollection.ReplaceOneAsync(filter, existingUser);
            }
        }
    }
}
