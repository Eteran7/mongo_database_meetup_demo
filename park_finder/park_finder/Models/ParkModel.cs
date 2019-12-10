using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

using Newtonsoft.Json;
using ParkFinder.Serializers;

namespace ParkFinder.Models
{
    public class ParkModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [JsonProperty("id")]
        public string Id { get; set; }

        [BsonElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [BsonElement("address")]
        [JsonProperty("address")]
        public AddressModel Address { get; set; }

        [BsonElement("geometry")]
        [JsonProperty("geometry")]
        [JsonConverter(typeof(MongoGeoJsonConverter<GeoJsonMultiPolygon<GeoJson2DGeographicCoordinates>>))]
        public GeoJsonMultiPolygon<GeoJson2DGeographicCoordinates> Geometry { get; set; }
    }
}
