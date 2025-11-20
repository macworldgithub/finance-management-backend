using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace finance_management_backend.Models
{
    public class ControlAssessment
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

        [BsonElement("Level of Responsibility-Operating Level (Entity / Activity)")]
        [JsonPropertyName("Level of Responsibility-Operating Level (Entity / Activity)")]
        public string LevelOfResponsibilityOperatingLevel { get; set; } = string.Empty;

        [BsonElement("COSO Principle #")]
        [JsonPropertyName("COSO Principle #")]
        public string CosoPrincipleNumber { get; set; } = string.Empty;

        [BsonElement("Operational Approach (Automated / Manual)")]
        [JsonPropertyName("Operational Approach (Automated / Manual)")]
        public string OperationalApproach { get; set; } = string.Empty;

        [BsonElement("Operational Frequency")]
        [JsonPropertyName("Operational Frequency")]
        public string OperationalFrequency { get; set; } = string.Empty;

        [BsonElement("Control Classification (Preventive / Detective / Corrective)")]
        [JsonPropertyName("Control Classification (Preventive / Detective / Corrective)")]
        public string ControlClassification { get; set; } = string.Empty;
    }
}
