using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System; // make sure this is at the top

namespace finance_management_backend.Models
{
    public class RiskAssessmentResidualRisk
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

        [BsonElement("Risk Type")]
        [JsonPropertyName("Risk Type")]
        public string RiskType { get; set; } = string.Empty;

        [BsonElement("Risk Description")]
        [JsonPropertyName("Risk Description")]
        public string RiskDescription { get; set; } = string.Empty;

        [BsonElement("Severity/ Impact")]
        [JsonPropertyName("Severity/ Impact")]
        public string SeverityImpact { get; set; } = string.Empty;

        [BsonElement("Probability/ Likelihood")]
        [JsonPropertyName("Probability/ Likelihood")]
        public string ProbabilityLikelihood { get; set; } = string.Empty;

        [BsonElement("Classification")]
        [JsonPropertyName("Classification")]
        public string Classification { get; set; } = string.Empty;
    }
}
