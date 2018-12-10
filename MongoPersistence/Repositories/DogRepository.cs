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
        IMongoCollection<IDogEntity> _dogCollection;

        public DogRepository()
        {
            _connection = new MongoConnectionManager();
            _dogCollection = _connection.GetCollection<IDogEntity>("DogDomain");
        }
        
        public async void AddDog(IDogEntity dogEntity)
        {
            await _dogCollection.InsertOneAsync(dogEntity);
        }

        public async void DeleteDog(IDogEntity dogToDelete)
        {
            string dogId = dogToDelete.DogID;
            await _dogCollection.DeleteOneAsync<IDogEntity>(p => p.DogID.Equals(dogId));
        }

        public async Task<IDogEntity> GetDog(string dogId)
        {
            var result = await _dogCollection.Find<IDogEntity>(p => p.DogID.Equals(dogId)).FirstOrDefaultAsync();
            return await Task.FromResult(result);
        }

        public void UpdateDog(IDogEntity dogToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
