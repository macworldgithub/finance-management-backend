using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace finance_management_backend.Models
{
    public class InternalAuditTest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("Id")]
        public string? Id { get; set; }

        [BsonElement("No")]
        [JsonPropertyName("No")]
        public double No { get; set; }

        [BsonElement("Process")]
        [JsonPropertyName("Process")]
        public string Process { get; set; } = string.Empty;

        [BsonElement("Check")]
        [JsonPropertyName("Check")]
        public string Check { get; set; } = string.Empty;

        [BsonElement("Internal Audit Test")]
        [JsonPropertyName("Internal Audit Test")]
        public string InternalAuditTestName { get; set; } = string.Empty;

        [BsonElement("Sample Size")]
        [JsonPropertyName("Sample Size")]
        public string SampleSize { get; set; } = string.Empty;
    }
}
