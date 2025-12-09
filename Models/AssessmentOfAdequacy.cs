// Models/AssessmentOfAdequacy.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace finance_management_backend.Models
{
    public class AssessmentOfAdequacy
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

        [BsonElement("DesignAdequacyScore")]
        [JsonPropertyName("DesignAdequacyScore")]
        public double DesignAdequacyScore { get; set; } // 0-10

        [BsonElement("SustainabilityScore")]
        [JsonPropertyName("SustainabilityScore")]
        public double SustainabilityScore { get; set; } // 0-10

        [BsonElement("ScalabilityScore")]
        [JsonPropertyName("ScalabilityScore")]
        public double ScalabilityScore { get; set; } // 0-5

        [BsonElement("AdequacyScore")]
        [JsonPropertyName("AdequacyScore")]
        public double AdequacyScore { get; set; } // 0-25 (calculated)

        [BsonElement("TotalScore")]
        [JsonPropertyName("TotalScore")]
        public double TotalScore { get; set; } // same as AdequacyScore

        [BsonElement("Scale")]
        [JsonPropertyName("Scale")]
        public int Scale { get; set; } // 1-5

        [BsonElement("Rating")]
        [JsonPropertyName("Rating")]
        public string Rating { get; set; } = string.Empty; // Now a free string
    }
}