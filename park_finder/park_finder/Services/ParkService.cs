using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

using ParkFinder.Models;

namespace ParkFinder.Services
{
    public interface IParkService
    {
        Task<List<ParkModel>> RetrieveByGuids(Guid[] guids);
    }

    public class ParkService : IParkService
    {
        private readonly IMongoCollection<ParkModel> _collection;

        public ParkService(IMongoClient client)
        {
            var database = client.GetDatabase("meetup");
            _collection = database.GetCollection<ParkModel>("park");
        }

        public async Task<List<ParkModel>> RetrieveByGuids(Guid[] guids)
        {
            var filterDefinition = Builders<ParkModel>.Filter.AnyIn("_id", guids.Select(guid => guid.ToString()));
            return (await _collection.FindAsync(filterDefinition)).ToList();
        }
    }
}
