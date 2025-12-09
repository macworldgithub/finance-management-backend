using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace finance_management_backend.Models
{
    public class ProcessSeverity
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

        [BsonElement("Date")]
        [JsonPropertyName("Date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Process Severity Level: 1 = Low, 2 = Medium, 3 = High, 4 = Critical
        /// </summary>
        [BsonElement("Scale")]
        [JsonPropertyName("Scale")]
        public int Scale { get; set; } // 1 to 4 only

        [BsonElement("Rating")]
        [JsonPropertyName("Rating")]
        public string Rating { get; set; } = string.Empty; // "Low", "Medium", "High", "Critical"
    }
}