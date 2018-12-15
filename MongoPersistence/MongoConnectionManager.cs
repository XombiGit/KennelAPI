using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoPersistence
{
    //TODO transform into Singleton
    public class MongoConnectionManager
    {
        private readonly IMongoDatabase _mongoDb;

        public MongoConnectionManager()
        {
            _mongoDb = Initialize("mongodb://Xombi:$BigHero6@ds041167.mlab.com:41167/xombikennel");
        }

        private IMongoDatabase Initialize(string connectionString)
        {
            var connection = MongoUrl.Create(connectionString);
            var settings = MongoClientSettings.FromUrl(connection);
            var client = new MongoClient(settings);

            return client.GetDatabase(connection.DatabaseName);
        }

        public IMongoDatabase GetDataBase()
        {
            return _mongoDb;
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            try
            {
                return _mongoDb.GetCollection<T>(collectionName);
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
