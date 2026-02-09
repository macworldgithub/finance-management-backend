using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System;

namespace finance_management_backend.Models
{
    public class RiskResponseScoring
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("Id")]
        public string? Id { get; set; }

        [BsonElement("Date")]
        [JsonPropertyName("Date")]
        public DateTime Date { get; set; }

        [BsonElement("No")]
        [JsonPropertyName("No")]
        public double No { get; set; }

        [BsonElement("Process")]
        [JsonPropertyName("Process")]
        public string Process { get; set; } = string.Empty;

        // 0 or 1 values
        [BsonElement("Avoid")]
        [JsonPropertyName("Avoid")]
        public int Avoid { get; set; } = 0;

        [BsonElement("Mitigate")]
        [JsonPropertyName("Mitigate")]
        public int Mitigate { get; set; } = 0;

        [BsonElement("Transfer")]
        [JsonPropertyName("Transfer")]
        public int Transfer { get; set; } = 0;

        [BsonElement("Share")]
        [JsonPropertyName("Share")]
        public int Share { get; set; } = 0;

        [BsonElement("Accept")]
        [JsonPropertyName("Accept")]
        public int Accept { get; set; } = 0;

        // Individual total scores
        [BsonElement("TotalScoreAvoid")]
        [JsonPropertyName("TotalScoreAvoid")]
        public int TotalScoreAvoid { get; set; } = 0;

        [BsonElement("TotalScoreMitigate")]
        [JsonPropertyName("TotalScoreMitigate")]
        public int TotalScoreMitigate { get; set; } = 0;

        [BsonElement("TotalScoreTransfer")]
        [JsonPropertyName("TotalScoreTransfer")]
        public int TotalScoreTransfer { get; set; } = 0;

        [BsonElement("TotalScoreShare")]
        [JsonPropertyName("TotalScoreShare")]
        public int TotalScoreShare { get; set; } = 0;

        [BsonElement("TotalScoreAccept")]
        [JsonPropertyName("TotalScoreAccept")]
        public int TotalScoreAccept { get; set; } = 0;
    }
}
