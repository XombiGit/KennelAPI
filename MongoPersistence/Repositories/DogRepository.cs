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
    public class DogRepository :IDogRepository
    {
        MongoConnectionManager _connection;
        IMongoCollection<DogEntity> _dogCollection;

        public DogRepository()
        {
            _connection = new MongoConnectionManager();
            _dogCollection = _connection.GetCollection<DogEntity>("DogDomain");
        }
        
        public async void AddDog(IDogEntity dogEntity)
        {
            DogEntity dog = (DogEntity) dogEntity;
            await _dogCollection.InsertOneAsync(dog);
        }

        public async void DeleteDog(IDogEntity dogToDelete)
        {
            string dogId = dogToDelete.DogID;
            await _dogCollection.DeleteOneAsync<DogEntity>(p => p.DogID.Equals(dogId));
        }

        public async Task<IDogEntity> GetDog(string dogId)
        {
            //IMongoCollection<DogEntity> dogEntity = (IMongoCollection<DogEntity>)_dogCollection;
            var result = await _dogCollection.Find<DogEntity>(p => p.DogID == dogId).FirstOrDefaultAsync();
            return result;
        }

        public void UpdateDog(IDogEntity dogToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
