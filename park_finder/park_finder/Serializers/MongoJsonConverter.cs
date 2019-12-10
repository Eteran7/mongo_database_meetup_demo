using System;
using System.Collections.Generic;

using GeoJSON.Net.Geometry;

using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ParkFinder.Serializers
{
    public class MongoGeoJsonConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var jObjectString = JObject.Load(reader).ToString();

            if (objectType == typeof(GeoJsonPoint<GeoJson2DGeographicCoordinates>))
                return _parseGeoJsonPoint(jObjectString);
            else if (objectType == typeof(GeoJsonMultiPolygon<GeoJson2DGeographicCoordinates>))
                return _parseGeoJsonMultiPolygon(jObjectString);
            else
                throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is T geometry)
                writer.WriteRawValue(geometry.ToJson());
            else
                throw new NotImplementedException();
        }

        private GeoJsonPoint<GeoJson2DGeographicCoordinates> _parseGeoJsonPoint(string value)
        {
            var point = JsonConvert.DeserializeObject<Point>(value);
            return GeoJson.Point(new GeoJson2DGeographicCoordinates(point.Coordinates.Longitude, point.Coordinates.Latitude));
        }

        private GeoJsonMultiPolygon<GeoJson2DGeographicCoordinates> _parseGeoJsonMultiPolygon(string value)
        {
            var multiPolygon = JsonConvert.DeserializeObject<MultiPolygon>(value);
            var polygonCoordinates = new List<GeoJsonPolygonCoordinates<GeoJson2DGeographicCoordinates>>();

            foreach (var polygon in multiPolygon.Coordinates)
            {
                var geographicCoordinates = new List<GeoJson2DGeographicCoordinates>();

                foreach (var lineString in polygon.Coordinates)
                {
                    foreach (var position in lineString.Coordinates)
                    {
                        geographicCoordinates.Add(new GeoJson2DGeographicCoordinates(position.Longitude, position.Latitude));
                    }
                }

                polygonCoordinates.Add(GeoJson.PolygonCoordinates(geographicCoordinates.ToArray()));
            }

            return GeoJson.MultiPolygon(polygonCoordinates.ToArray());
        }
    }
}
 