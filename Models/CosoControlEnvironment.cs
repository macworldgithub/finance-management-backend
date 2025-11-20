using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace finance_management_backend.Models
{
    public class CosoControlEnvironment
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

        [BsonElement("Integrity & Ethical Values")]
        [JsonPropertyName("Integrity & Ethical Values")]
        public string IntegrityAndEthicalValues { get; set; } = string.Empty;

        [BsonElement("Board Oversight")]
        [JsonPropertyName("Board Oversight")]
        public string BoardOversight { get; set; } = string.Empty;

        [BsonElement("Organizational Structure")]
        [JsonPropertyName("Organizational Structure")]
        public string OrganizationalStructure { get; set; } = string.Empty;

        [BsonElement("Commitment to Competence")]
        [JsonPropertyName("Commitment to Competence")]
        public string CommitmentToCompetence { get; set; } = string.Empty;

        [BsonElement("Management Philosophy")]
        [JsonPropertyName("Management Philosophy")]
        public string ManagementPhilosophy { get; set; } = string.Empty;
    }
}
