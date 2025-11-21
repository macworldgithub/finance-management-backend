using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System; // make sure this is at the top

namespace finance_management_backend.Models
{
    public class FinancialStatementAssertion
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

        [BsonElement("Internal Control Over Financial Reporting?")]
        [JsonPropertyName("Internal Control Over Financial Reporting?")]
        public string InternalControlOverFinancialReporting { get; set; } = string.Empty;

        [BsonElement("Occurrence")]
        [JsonPropertyName("Occurrence")]
        public string Occurrence { get; set; } = string.Empty;

        [BsonElement("Completeness")]
        [JsonPropertyName("Completeness")]
        public string Completeness { get; set; } = string.Empty;

        [BsonElement("Accuracy")]
        [JsonPropertyName("Accuracy")]
        public string Accuracy { get; set; } = string.Empty;

        [BsonElement("Authorization")]
        [JsonPropertyName("Authorization")]
        public string Authorization { get; set; } = string.Empty;

        [BsonElement("Cutoff")]
        [JsonPropertyName("Cutoff")]
        public string Cutoff { get; set; } = string.Empty;

        [BsonElement("Classification and Understandability")]
        [JsonPropertyName("Classification and Understandability")]
        public string ClassificationAndUnderstandability { get; set; } = string.Empty;

        [BsonElement("Existence")]
        [JsonPropertyName("Existence")]
        public string Existence { get; set; } = string.Empty;

        [BsonElement("Rights and Obligations")]
        [JsonPropertyName("Rights and Obligations")]
        public string RightsAndObligations { get; set; } = string.Empty;

        [BsonElement("Valuation and Allocation")]
        [JsonPropertyName("Valuation and Allocation")]
        public string ValuationAndAllocation { get; set; } = string.Empty;

        [BsonElement("Presentation / Disclosure")]
        [JsonPropertyName("Presentation / Disclosure")]
        public string PresentationDisclosure { get; set; } = string.Empty;
    }
}
