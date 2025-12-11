// Models/AssessmentOfEffectiveness.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace finance_management_backend.Models
{
    public class AssessmentOfEffectiveness
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

        [BsonElement("DesignScore")]
        [JsonPropertyName("DesignScore")]
        public double DesignScore { get; set; } // 0-10

        [BsonElement("OperatingScore")]
        [JsonPropertyName("OperatingScore")]
        public double OperatingScore { get; set; } // 0-10

        [BsonElement("SustainabilityScore")]
        [JsonPropertyName("SustainabilityScore")]
        public double SustainabilityScore { get; set; } // 0-5

        [BsonElement("EffectivenessScore")]
        [JsonPropertyName("EffectivenessScore")]
        public double EffectivenessScore { get; set; } // 0-25

        [BsonElement("TotalScore")]
[JsonPropertyName("TotalScore")]
public string TotalScore { get; set; } = string.Empty; // Now string

        [BsonElement("Scale")]
        [JsonPropertyName("Scale")]
        public int Scale { get; set; } // 1-5

        [BsonElement("Rating")]
        [JsonPropertyName("Rating")]
        public string Rating { get; set; } = string.Empty; // Now string
    }
}