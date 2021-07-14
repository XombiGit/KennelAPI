using Common.Entities;
using Common.Interfaces;
using Common.Interfaces.Services;
using MongoDB.Driver;
using MongoPersistence.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoPersistence.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        MongoConnectionManager _connection;
        IMongoCollection<LocationEntity> _locationCollection;
        ILocationService locate;
        const int RADIUS = 50;

        public LocationRepository()
        {
            _connection = new MongoConnectionManager();
            _locationCollection = _connection.GetCollection<LocationEntity>("LocationDomain");
        }
        public async Task<List<ILocationEntity>> GetNearbyDogs(string dogId)
        {
            var result = await _locationCollection.Find<LocationEntity>(p => p.DogID == dogId).FirstOrDefaultAsync();
            var x = result.XCoord;
            var y = result.YCoord;

            //d references a row in location collection
            var result0 = await _locationCollection.Find<LocationEntity>(d => locate.MeasureDistance(result, d) < RADIUS).ToListAsync();

            return result0.ConvertAll(d => (ILocationEntity)d);
        }

        public void UpdateLocation(ILocationEntity locationToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
