using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace finance_management_backend.Models
{
    public class OtherControlEnvironment
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

        [BsonElement("Responsibility Delegation Matrix")]
        [JsonPropertyName("Responsibility Delegation Matrix")]
        public string ResponsibilityDelegationMatrix { get; set; } = string.Empty;

        [BsonElement("Segregation of duties")]
        [JsonPropertyName("Segregation of duties")]
        public string SegregationOfDuties { get; set; } = string.Empty;

        [BsonElement("Reporting Lines")]
        [JsonPropertyName("Reporting Lines")]
        public string ReportingLines { get; set; } = string.Empty;

        [BsonElement("Mission")]
        [JsonPropertyName("Mission")]
        public string Mission { get; set; } = string.Empty;

        [BsonElement("Vision and Values")]
        [JsonPropertyName("Vision and Values")]
        public string VisionAndValues { get; set; } = string.Empty;

        [BsonElement("Goals and Objectives")]
        [JsonPropertyName("Goals and Objectives")]
        public string GoalsAndObjectives { get; set; } = string.Empty;

        [BsonElement("Structures & Systems")]
        [JsonPropertyName("Structures & Systems")]
        public string StructuresAndSystems { get; set; } = string.Empty;

        [BsonElement("Policies and Procedures")]
        [JsonPropertyName("Policies and Procedures")]
        public string PoliciesAndProcedures { get; set; } = string.Empty;

        [BsonElement("Processes")]
        [JsonPropertyName("Processes")]
        public string Processes { get; set; } = string.Empty;

        [BsonElement("Integrity and Ethical Values")]
        [JsonPropertyName("Integrity and Ethical Values")]
        public string IntegrityAndEthicalValues { get; set; } = string.Empty;

        [BsonElement("Oversight structure")]
        [JsonPropertyName("Oversight structure")]
        public string OversightStructure { get; set; } = string.Empty;

        [BsonElement("Standards")]
        [JsonPropertyName("Standards")]
        public string Standards { get; set; } = string.Empty;

        [BsonElement("Methodologies")]
        [JsonPropertyName("Methodologies")]
        public string Methodologies { get; set; } = string.Empty;

        [BsonElement("Rules and Regulations")]
        [JsonPropertyName("Rules and Regulations")]
        public string RulesAndRegulations { get; set; } = string.Empty;
    }
}
