using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MongoDB.Driver;
using MongoDB.Bson;

using ParkFinder.Models;

namespace ParkFinder.Services
{
    public interface IParkService
    {
        Task<ParkModel> Create(ParkModel park);
        Task<ParkModel> Retrieve(Guid guid);
        Task<ParkModel> Update(Guid guid, ParkModel park);
        Task<ParkModel> Delete(Guid guid);

        Task<List<ParkModel>> FindNear(double longitude, double latitude, double threshold);
    }

    public class ParkService : IParkService
    {
        private readonly IMongoCollection<ParkModel> _collection;

        public ParkService(IMongoClient client)
        {
            var database = client.GetDatabase("meetup");
            _collection = database.GetCollection<ParkModel>("park");
        }

        public async Task<ParkModel> Create(ParkModel park)
        {
            try
            {
                park.Id = Guid.NewGuid().ToString();
                await _collection.InsertOneAsync(park);

                return park;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ParkModel> Retrieve(Guid guid)
        {
            var filterDefinition = Builders<ParkModel>.Filter.Eq("_id", guid.ToString());
            return (await _collection.FindAsync(filterDefinition)).FirstOrDefault();
        }

        public async Task<ParkModel> Update(Guid guid, ParkModel park)
        {
            var updateOptions = new FindOneAndUpdateOptions<ParkModel>()
            {
                ReturnDocument = ReturnDocument.After
            };

            var filterDefinition = Builders<ParkModel>.Filter.Eq("_id", guid.ToString());
            var updateDefinition = Builders<ParkModel>.Update.Set("name", park.Name).
                                                              Set("address", park.Address);

            return await _collection.FindOneAndUpdateAsync(filterDefinition, updateDefinition, updateOptions);
        }

        public async Task<ParkModel> Delete(Guid guid)
        {
            var filterDefinition = Builders<ParkModel>.Filter.Eq("_id", guid.ToString());
            return await _collection.FindOneAndDeleteAsync(filterDefinition);
        }

        public async Task<List<ParkModel>> FindNear(double longitude, double latitude, double threshold)
        {
            var pipeline = new List<BsonDocument>()
            {
                new BsonDocument("$geoNear", new BsonDocument{
                { "near", new BsonDocument
                {
                    { "type", "Point" },
                    { "coordinates", new BsonArray{ longitude, latitude } }
                } },
                { "distanceField", "distance" },
                { "maxDistance", threshold },
                { "spherical", true }
                })
            };

            return (await _collection.AggregateAsync((PipelineDefinition<ParkModel, ParkModel>)pipeline)).ToList();
        }
    }
}
