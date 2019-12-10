using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

using Newtonsoft.Json;
using ParkFinder.Serializers;

namespace ParkFinder.Models
{
    [BsonIgnoreExtraElements]
    public class AddressModel
    {
        [BsonElement("geometry")]
        [JsonProperty("geometry")]
        [JsonConverter(typeof(MongoGeoJsonConverter<GeoJsonPoint<GeoJson2DGeographicCoordinates>>))]
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Geometry { get; set; }

        [BsonElement("street")]
        [JsonProperty("street")]
        public string Street { get; set; }

        [BsonElement("city")]
        [JsonProperty("city")]
        public string City { get; set; }

        [BsonElement("state")]
        [JsonProperty("state")]
        public string State { get; set; }

        [BsonElement("zip")]
        [JsonProperty("zip")]
        public string Zip { get; set; }

        [BsonElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }
    }
}
