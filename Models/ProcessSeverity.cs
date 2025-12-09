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

        // Severity scoring fields (you can adjust names/ranges as needed)
        [BsonElement("ImpactScore")]
        [JsonPropertyName("ImpactScore")]
        public double ImpactScore { get; set; } // e.g. 0–10

        [BsonElement("LikelihoodScore")]
        [JsonPropertyName("LikelihoodScore")]
        public double LikelihoodScore { get; set; } // e.g. 0–10

        [BsonElement("DetectabilityScore")]
        [JsonPropertyName("DetectabilityScore")]
        public double DetectabilityScore { get; set; } // e.g. 0–5 (optional)

        [BsonElement("SeverityScore")]
        [JsonPropertyName("SeverityScore")]
        public double SeverityScore { get; set; } // calculated, e.g. Impact × Likelihood

        [BsonElement("TotalScore")]
        [JsonPropertyName("TotalScore")]
        public double TotalScore { get; set; } // same as SeverityScore (kept for consistency)

        /// <summary>
        /// Severity Level: 1 = Low, 2 = Moderate, 3 = High, 4 = Critical
        /// </summary>
        [BsonElement("Scale")]
        [JsonPropertyName("Scale")]
        public int Scale { get; set; } // 1–4

        [BsonElement("Rating")]
        [JsonPropertyName("Rating")]
        public string Rating { get; set; } = string.Empty; // e.g. "Low", "Moderate", "High", "Critical"
    }
}