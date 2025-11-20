using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace finance_management_backend.Models
{
    public class IntosaiIfacControlEnvironment
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

        [BsonElement("Integrity and Ethical Values")]
        [JsonPropertyName("Integrity and Ethical Values")]
        public string IntegrityAndEthicalValues { get; set; } = string.Empty;

        [BsonElement("Commitment to Competence")]
        [JsonPropertyName("Commitment to Competence")]
        public string CommitmentToCompetence { get; set; } = string.Empty;

        [BsonElement("Management’s Philosophy and Operating Style")]
        [JsonPropertyName("Management’s Philosophy and Operating Style")]
        public string ManagementsPhilosophyAndOperatingStyle { get; set; } = string.Empty;

        [BsonElement("Organizational Structure")]
        [JsonPropertyName("Organizational Structure")]
        public string OrganizationalStructure { get; set; } = string.Empty;

        [BsonElement("Assignment of Authority and Responsibility")]
        [JsonPropertyName("Assignment of Authority and Responsibility")]
        public string AssignmentOfAuthorityAndResponsibility { get; set; } = string.Empty;

        [BsonElement("Human Resource Policies and Practices")]
        [JsonPropertyName("Human Resource Policies and Practices")]
        public string HumanResourcePoliciesAndPractices { get; set; } = string.Empty;

        [BsonElement("Board of Directors’ or Audit Committee’s Participation")]
        [JsonPropertyName("Board of Directors’ or Audit Committee’s Participation")]
        public string BoardOrAuditCommitteeParticipation { get; set; } = string.Empty;

        [BsonElement("Management Control Methods")]
        [JsonPropertyName("Management Control Methods")]
        public string ManagementControlMethods { get; set; } = string.Empty;

        [BsonElement("External Influences")]
        [JsonPropertyName("External Influences")]
        public string ExternalInfluences { get; set; } = string.Empty;

        [BsonElement("Management’s Commitment to Internal Control")]
        [JsonPropertyName("Management’s Commitment to Internal Control")]
        public string ManagementsCommitmentToInternalControl { get; set; } = string.Empty;

        [BsonElement("Communication and Enforcement of Integrity and Ethical Values")]
        [JsonPropertyName("Communication and Enforcement of Integrity and Ethical Values")]
        public string CommunicationAndEnforcementOfIntegrityAndEthicalValues { get; set; } = string.Empty;

        [BsonElement("Employee Awareness and Understanding")]
        [JsonPropertyName("Employee Awareness and Understanding")]
        public string EmployeeAwarenessAndUnderstanding { get; set; } = string.Empty;

        [BsonElement("Accountability and Performance Measurement")]
        [JsonPropertyName("Accountability and Performance Measurement")]
        public string AccountabilityAndPerformanceMeasurement { get; set; } = string.Empty;

        [BsonElement("Commitment to Transparency and Openness")]
        [JsonPropertyName("Commitment to Transparency and Openness")]
        public string CommitmentToTransparencyAndOpenness { get; set; } = string.Empty;
    }
}
