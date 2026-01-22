using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System;

namespace finance_management_backend.Models
{
    public class ControlEnvironmentScoring
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
        // 1. Integrity & Ethical Values
        [BsonElement("IntegrityEthicalValues")]
        [JsonPropertyName("IntegrityEthicalValues")]
        public string IntegrityEthicalValues { get; set; } = string.Empty;

        [BsonElement("IntegrityDesignScore")]
        [JsonPropertyName("IntegrityDesignScore")]
        public double IntegrityDesignScore { get; set; } // 0-10

        [BsonElement("IntegrityPerformanceScore")]
        [JsonPropertyName("IntegrityPerformanceScore")]
        public double IntegrityPerformanceScore { get; set; } // 0-10

        [BsonElement("IntegritySustainabilityScore")]
        [JsonPropertyName("IntegritySustainabilityScore")]
        public double IntegritySustainabilityScore { get; set; } // 0-5

        [BsonElement("IntegrityTotalScore")]
        [JsonPropertyName("IntegrityTotalScore")]
        public string IntegrityTotalScore { get; set; } = string.Empty;

        [BsonElement("IntegrityScale")]
        [JsonPropertyName("IntegrityScale")]
        public int IntegrityScale { get; set; } // 1-5

        [BsonElement("IntegrityRating")]
        [JsonPropertyName("IntegrityRating")]
        public string IntegrityRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 2. Board Oversight
        [BsonElement("BoardOversight")]
        [JsonPropertyName("BoardOversight")]
        public string BoardOversight { get; set; } = string.Empty;

        [BsonElement("BoardDesignScore")]
        [JsonPropertyName("BoardDesignScore")]
        public double BoardDesignScore { get; set; } // 0-10

        [BsonElement("BoardPerformanceScore")]
        [JsonPropertyName("BoardPerformanceScore")]
        public double BoardPerformanceScore { get; set; } // 0-10

        [BsonElement("BoardSustainabilityScore")]
        [JsonPropertyName("BoardSustainabilityScore")]
        public double BoardSustainabilityScore { get; set; } // 0-5

        [BsonElement("BoardTotalScore")]
        [JsonPropertyName("BoardTotalScore")]
        public string BoardTotalScore { get; set; } = string.Empty;

        [BsonElement("BoardScale")]
        [JsonPropertyName("BoardScale")]
        public int BoardScale { get; set; } // 1-5

        [BsonElement("BoardRating")]
        [JsonPropertyName("BoardRating")]
        public string BoardRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 3. Organizational Structure
        [BsonElement("OrganizationalStructure")]
        [JsonPropertyName("OrganizationalStructure")]
        public string OrganizationalStructure { get; set; } = string.Empty;

        [BsonElement("OrgStructureDesignScore")]
        [JsonPropertyName("OrgStructureDesignScore")]
        public double OrgStructureDesignScore { get; set; } // 0-10

        [BsonElement("OrgStructurePerformanceScore")]
        [JsonPropertyName("OrgStructurePerformanceScore")]
        public double OrgStructurePerformanceScore { get; set; } // 0-10

        [BsonElement("OrgStructureSustainabilityScore")]
        [JsonPropertyName("OrgStructureSustainabilityScore")]
        public double OrgStructureSustainabilityScore { get; set; } // 0-5

        [BsonElement("OrgStructureTotalScore")]
        [JsonPropertyName("OrgStructureTotalScore")]
        public string OrgStructureTotalScore { get; set; } = string.Empty;

        [BsonElement("OrgStructureScale")]
        [JsonPropertyName("OrgStructureScale")]
        public int OrgStructureScale { get; set; } // 1-5

        [BsonElement("OrgStructureRating")]
        [JsonPropertyName("OrgStructureRating")]
        public string OrgStructureRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 4. Commitment to Competence
        [BsonElement("CommitmentToCompetence")]
        [JsonPropertyName("CommitmentToCompetence")]
        public string CommitmentToCompetence { get; set; } = string.Empty;

        [BsonElement("CompetenceDesignScore")]
        [JsonPropertyName("CompetenceDesignScore")]
        public double CompetenceDesignScore { get; set; } // 0-10

        [BsonElement("CompetencePerformanceScore")]
        [JsonPropertyName("CompetencePerformanceScore")]
        public double CompetencePerformanceScore { get; set; } // 0-10

        [BsonElement("CompetenceSustainabilityScore")]
        [JsonPropertyName("CompetenceSustainabilityScore")]
        public double CompetenceSustainabilityScore { get; set; } // 0-5

        [BsonElement("CompetenceTotalScore")]
        [JsonPropertyName("CompetenceTotalScore")]
        public string CompetenceTotalScore { get; set; } = string.Empty;

        [BsonElement("CompetenceScale")]
        [JsonPropertyName("CompetenceScale")]
        public int CompetenceScale { get; set; } // 1-5

        [BsonElement("CompetenceRating")]
        [JsonPropertyName("CompetenceRating")]
        public string CompetenceRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 5. Management Philosophy
        [BsonElement("ManagementPhilosophy")]
        [JsonPropertyName("ManagementPhilosophy")]
        public string ManagementPhilosophy { get; set; } = string.Empty;

        [BsonElement("PhilosophyDesignScore")]
        [JsonPropertyName("PhilosophyDesignScore")]
        public double PhilosophyDesignScore { get; set; } // 0-10

        [BsonElement("PhilosophyPerformanceScore")]
        [JsonPropertyName("PhilosophyPerformanceScore")]
        public double PhilosophyPerformanceScore { get; set; } // 0-10

        [BsonElement("PhilosophySustainabilityScore")]
        [JsonPropertyName("PhilosophySustainabilityScore")]
        public double PhilosophySustainabilityScore { get; set; } // 0-5

        [BsonElement("PhilosophyTotalScore")]
        [JsonPropertyName("PhilosophyTotalScore")]
        public string PhilosophyTotalScore { get; set; } = string.Empty;

        [BsonElement("PhilosophyScale")]
        [JsonPropertyName("PhilosophyScale")]
        public int PhilosophyScale { get; set; } // 1-5

        [BsonElement("PhilosophyRating")]
        [JsonPropertyName("PhilosophyRating")]
        public string PhilosophyRating { get; set; } = string.Empty;
    }
}