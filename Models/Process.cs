using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace finance_management_backend.Models
{
    public class Process
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("Id")]
        public string? Id { get; set; }

        // "No" is double like 5.1
        [BsonElement("No")]
        [JsonPropertyName("No")]
        public double No { get; set; }

        [BsonElement("Process")]
        [JsonPropertyName("Process")]
        public string ProcessName { get; set; } = string.Empty;

        [BsonElement("Process Description")]
        [JsonPropertyName("Process Description")]
        public string ProcessDescription { get; set; } = string.Empty;

        [BsonElement("Process Objectives")]
        [JsonPropertyName("Process Objectives")]
        public string ProcessObjectives { get; set; } = string.Empty;

        [BsonElement("Process Severity Levels")]
        [JsonPropertyName("Process Severity Levels")]
        public string ProcessSeverityLevels { get; set; } = string.Empty;
    }
}
