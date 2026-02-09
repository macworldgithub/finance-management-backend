using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System;

namespace finance_management_backend.Models
{
    public class RiskAssessmentInherentRiskScoring
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

        // ───────────────────────────────────────────────
        // Risk Identification

        [BsonElement("RiskId")]
        [JsonPropertyName("RiskId")]
        public string RiskId { get; set; } = string.Empty;

        [BsonElement("RiskType")]
        [JsonPropertyName("RiskType")]
        public string RiskType { get; set; } = string.Empty;

        [BsonElement("RiskDescription")]
        [JsonPropertyName("RiskDescription")]
        public string RiskDescription { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // Inherent Risk Scoring (General)

        [BsonElement("SeverityImpact")]
        [JsonPropertyName("SeverityImpact")]
        public String SeverityImpact { get; set; }

        [BsonElement("ProbabilityLikelihood")]
        [JsonPropertyName("ProbabilityLikelihood")]
        public String ProbabilityLikelihood { get; set; }

        [BsonElement("Classification")]
        [JsonPropertyName("Classification")]
        public String Classification { get; set; }

        // ───────────────────────────────────────────────
        // Inherent Risk Scoring (RiskId Based)

        [BsonElement("RiskIdSeverityImpact")]
        [JsonPropertyName("RiskIdSeverityImpact")]
        public double RiskIdSeverityImpact { get; set; }

        [BsonElement("RiskIdProbabilityLikelihood")]
        [JsonPropertyName("RiskIdProbabilityLikelihood")]
        public double RiskIdProbabilityLikelihood { get; set; }

        [BsonElement("RiskIdClassification")]
        [JsonPropertyName("RiskIdClassification")]
        public double RiskIdClassification { get; set; }
    }
}
