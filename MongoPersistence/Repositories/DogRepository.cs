using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;
using Common.Interfaces;
using Common.Interfaces.Services;
using MongoDB.Driver;
using MongoPersistence.Entities;

namespace MongoPersistence.Services
{
    public class DogRepository :IDogRepository
    {
        MongoConnectionManager _connection;
        IMongoCollection<DogEntity> _dogCollection;

        public DogRepository()
        {
            _connection = new MongoConnectionManager();
            _dogCollection = _connection.GetCollection<DogEntity>("DogDomain");
        }
        
        public async Task AddDog(IDogEntity dogEntity)
        {
            DogEntity dog = (DogEntity) dogEntity; 
            await _dogCollection.InsertOneAsync(dog);
        }

        public async void DeleteDog(IDogEntity dogEntity)
        {
            DogEntity dogToDelete = (DogEntity) dogEntity;
            string dogId = dogToDelete.DogID;
            await _dogCollection.DeleteOneAsync<DogEntity>(p => p.DogID == dogId);
        }

        public async Task<IDogEntity> GetDog(string dogId)
        {
            //IMongoCollection<DogEntity> dogEntity = (IMongoCollection<DogEntity>)_dogCollection;
            var result = await _dogCollection.Find<DogEntity>(p => p.DogID == dogId).FirstOrDefaultAsync();
            return result;
        }

        public async void UpdateDog(IDogEntity dogToUpdate)
        {
            //var existingDog = await GetDog(dogToUpdate.DogID);
            DogEntity existingDog = (DogEntity) dogToUpdate;
            string dogId = existingDog.DogID;
            string breed = existingDog.Breed;

            var filter = Builders<DogEntity>.Filter.Eq(m => m.DogID, dogId);

            if (dogToUpdate != null)
            {
                //var update = Builders<DogEntity>.Update.Set(m => m, existingDog);
                await _dogCollection.ReplaceOneAsync(filter, existingDog);
            }
        }
    }
}
