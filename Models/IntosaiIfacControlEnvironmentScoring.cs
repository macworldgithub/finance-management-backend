using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System;

namespace finance_management_backend.Models
{
    public class IntosaiIfacControlEnvironmentScoring
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
        // 1. Integrity and Ethical Values
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
        // 2. Commitment to Competence
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
        // 3. Management’s Philosophy and Operating Style
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

        // ───────────────────────────────────────────────
        // 4. Organizational Structure
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
        // 5. Assignment of Authority and Responsibility
        [BsonElement("AssignmentOfAuthority")]
        [JsonPropertyName("AssignmentOfAuthorityAndResponsibility")]
        public string AssignmentOfAuthority { get; set; } = string.Empty;

        [BsonElement("AuthorityDesignScore")]
        [JsonPropertyName("AuthorityDesignScore")]
        public double AuthorityDesignScore { get; set; } // 0-10

        [BsonElement("AuthorityPerformanceScore")]
        [JsonPropertyName("AuthorityPerformanceScore")]
        public double AuthorityPerformanceScore { get; set; } // 0-10

        [BsonElement("AuthoritySustainabilityScore")]
        [JsonPropertyName("AuthoritySustainabilityScore")]
        public double AuthoritySustainabilityScore { get; set; } // 0-5

        [BsonElement("AuthorityTotalScore")]
        [JsonPropertyName("AuthorityTotalScore")]
        public string AuthorityTotalScore { get; set; } = string.Empty;

        [BsonElement("AuthorityScale")]
        [JsonPropertyName("AuthorityScale")]
        public int AuthorityScale { get; set; } // 1-5

        [BsonElement("AuthorityRating")]
        [JsonPropertyName("AuthorityRating")]
        public string AuthorityRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 6. Human Resource Policies and Practices
        [BsonElement("HumanResourcePolicies")]
        [JsonPropertyName("HumanResourcePoliciesAndPractices")]
        public string HumanResourcePolicies { get; set; } = string.Empty;

        [BsonElement("HrDesignScore")]
        [JsonPropertyName("HrDesignScore")]
        public double HrDesignScore { get; set; } // 0-10

        [BsonElement("HrPerformanceScore")]
        [JsonPropertyName("HrPerformanceScore")]
        public double HrPerformanceScore { get; set; } // 0-10

        [BsonElement("HrSustainabilityScore")]
        [JsonPropertyName("HrSustainabilityScore")]
        public double HrSustainabilityScore { get; set; } // 0-5

        [BsonElement("HrTotalScore")]
        [JsonPropertyName("HrTotalScore")]
        public string HrTotalScore { get; set; } = string.Empty;

        [BsonElement("HrScale")]
        [JsonPropertyName("HrScale")]
        public int HrScale { get; set; } // 1-5

        [BsonElement("HrRating")]
        [JsonPropertyName("HrRating")]
        public string HrRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 7. Board of Directors’ or Audit Committee’s Participation
        [BsonElement("BoardParticipation")]
        [JsonPropertyName("BoardOrAuditCommitteeParticipation")]
        public string BoardParticipation { get; set; } = string.Empty;

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
        // 8. Management Control Methods
        [BsonElement("ManagementControlMethods")]
        [JsonPropertyName("ManagementControlMethods")]
        public string ManagementControlMethods { get; set; } = string.Empty;

        [BsonElement("ControlMethodsDesignScore")]
        [JsonPropertyName("ControlMethodsDesignScore")]
        public double ControlMethodsDesignScore { get; set; } // 0-10

        [BsonElement("ControlMethodsPerformanceScore")]
        [JsonPropertyName("ControlMethodsPerformanceScore")]
        public double ControlMethodsPerformanceScore { get; set; } // 0-10

        [BsonElement("ControlMethodsSustainabilityScore")]
        [JsonPropertyName("ControlMethodsSustainabilityScore")]
        public double ControlMethodsSustainabilityScore { get; set; } // 0-5

        [BsonElement("ControlMethodsTotalScore")]
        [JsonPropertyName("ControlMethodsTotalScore")]
        public string ControlMethodsTotalScore { get; set; } = string.Empty;

        [BsonElement("ControlMethodsScale")]
        [JsonPropertyName("ControlMethodsScale")]
        public int ControlMethodsScale { get; set; } // 1-5

        [BsonElement("ControlMethodsRating")]
        [JsonPropertyName("ControlMethodsRating")]
        public string ControlMethodsRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 9. External Influences
        [BsonElement("ExternalInfluences")]
        [JsonPropertyName("ExternalInfluences")]
        public string ExternalInfluences { get; set; } = string.Empty;

        [BsonElement("ExternalDesignScore")]
        [JsonPropertyName("ExternalDesignScore")]
        public double ExternalDesignScore { get; set; } // 0-10

        [BsonElement("ExternalPerformanceScore")]
        [JsonPropertyName("ExternalPerformanceScore")]
        public double ExternalPerformanceScore { get; set; } // 0-10

        [BsonElement("ExternalSustainabilityScore")]
        [JsonPropertyName("ExternalSustainabilityScore")]
        public double ExternalSustainabilityScore { get; set; } // 0-5

        [BsonElement("ExternalTotalScore")]
        [JsonPropertyName("ExternalTotalScore")]
        public string ExternalTotalScore { get; set; } = string.Empty;

        [BsonElement("ExternalScale")]
        [JsonPropertyName("ExternalScale")]
        public int ExternalScale { get; set; } // 1-5

        [BsonElement("ExternalRating")]
        [JsonPropertyName("ExternalRating")]
        public string ExternalRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 10. Management’s Commitment to Internal Control
        [BsonElement("ManagementCommitmentToIc")]
        [JsonPropertyName("ManagementCommitmentToInternalControl")]
        public string ManagementCommitmentToIc { get; set; } = string.Empty;

        [BsonElement("CommitmentIcDesignScore")]
        [JsonPropertyName("CommitmentIcDesignScore")]
        public double CommitmentIcDesignScore { get; set; } // 0-10

        [BsonElement("CommitmentIcPerformanceScore")]
        [JsonPropertyName("CommitmentIcPerformanceScore")]
        public double CommitmentIcPerformanceScore { get; set; } // 0-10

        [BsonElement("CommitmentIcSustainabilityScore")]
        [JsonPropertyName("CommitmentIcSustainabilityScore")]
        public double CommitmentIcSustainabilityScore { get; set; } // 0-5

        [BsonElement("CommitmentIcTotalScore")]
        [JsonPropertyName("CommitmentIcTotalScore")]
        public string CommitmentIcTotalScore { get; set; } = string.Empty;

        [BsonElement("CommitmentIcScale")]
        [JsonPropertyName("CommitmentIcScale")]
        public int CommitmentIcScale { get; set; } // 1-5

        [BsonElement("CommitmentIcRating")]
        [JsonPropertyName("CommitmentIcRating")]
        public string CommitmentIcRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 11. Communication and Enforcement of Integrity and Ethical Values
        [BsonElement("CommunicationEthicalValues")]
        [JsonPropertyName("CommunicationAndEnforcementOfIntegrityAndEthicalValues")]
        public string CommunicationEthicalValues { get; set; } = string.Empty;

        [BsonElement("CommEthicalDesignScore")]
        [JsonPropertyName("CommEthicalDesignScore")]
        public double CommEthicalDesignScore { get; set; } // 0-10

        [BsonElement("CommEthicalPerformanceScore")]
        [JsonPropertyName("CommEthicalPerformanceScore")]
        public double CommEthicalPerformanceScore { get; set; } // 0-10

        [BsonElement("CommEthicalSustainabilityScore")]
        [JsonPropertyName("CommEthicalSustainabilityScore")]
        public double CommEthicalSustainabilityScore { get; set; } // 0-5

        [BsonElement("CommEthicalTotalScore")]
        [JsonPropertyName("CommEthicalTotalScore")]
        public string CommEthicalTotalScore { get; set; } = string.Empty;

        [BsonElement("CommEthicalScale")]
        [JsonPropertyName("CommEthicalScale")]
        public int CommEthicalScale { get; set; } // 1-5

        [BsonElement("CommEthicalRating")]
        [JsonPropertyName("CommEthicalRating")]
        public string CommEthicalRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 12. Employee Awareness and Understanding
        [BsonElement("EmployeeAwareness")]
        [JsonPropertyName("EmployeeAwarenessAndUnderstanding")]
        public string EmployeeAwareness { get; set; } = string.Empty;

        [BsonElement("AwarenessDesignScore")]
        [JsonPropertyName("AwarenessDesignScore")]
        public double AwarenessDesignScore { get; set; } // 0-10

        [BsonElement("AwarenessPerformanceScore")]
        [JsonPropertyName("AwarenessPerformanceScore")]
        public double AwarenessPerformanceScore { get; set; } // 0-10

        [BsonElement("AwarenessSustainabilityScore")]
        [JsonPropertyName("AwarenessSustainabilityScore")]
        public double AwarenessSustainabilityScore { get; set; } // 0-5

        [BsonElement("AwarenessTotalScore")]
        [JsonPropertyName("AwarenessTotalScore")]
        public string AwarenessTotalScore { get; set; } = string.Empty;

        [BsonElement("AwarenessScale")]
        [JsonPropertyName("AwarenessScale")]
        public int AwarenessScale { get; set; } // 1-5

        [BsonElement("AwarenessRating")]
        [JsonPropertyName("AwarenessRating")]
        public string AwarenessRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 13. Accountability and Performance Measurement
        [BsonElement("AccountabilityPerformance")]
        [JsonPropertyName("AccountabilityAndPerformanceMeasurement")]
        public string AccountabilityPerformance { get; set; } = string.Empty;

        [BsonElement("AccountabilityDesignScore")]
        [JsonPropertyName("AccountabilityDesignScore")]
        public double AccountabilityDesignScore { get; set; } // 0-10

        [BsonElement("AccountabilityPerformanceScore")]
        [JsonPropertyName("AccountabilityPerformanceScore")]
        public double AccountabilityPerformanceScore { get; set; } // 0-10

        [BsonElement("AccountabilitySustainabilityScore")]
        [JsonPropertyName("AccountabilitySustainabilityScore")]
        public double AccountabilitySustainabilityScore { get; set; } // 0-5

        [BsonElement("AccountabilityTotalScore")]
        [JsonPropertyName("AccountabilityTotalScore")]
        public string AccountabilityTotalScore { get; set; } = string.Empty;

        [BsonElement("AccountabilityScale")]
        [JsonPropertyName("AccountabilityScale")]
        public int AccountabilityScale { get; set; } // 1-5

        [BsonElement("AccountabilityRating")]
        [JsonPropertyName("AccountabilityRating")]
        public string AccountabilityRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 14. Commitment to Transparency and Openness
        [BsonElement("TransparencyCommitment")]
        [JsonPropertyName("CommitmentToTransparencyAndOpenness")]
        public string TransparencyCommitment { get; set; } = string.Empty;

        [BsonElement("TransparencyDesignScore")]
        [JsonPropertyName("TransparencyDesignScore")]
        public double TransparencyDesignScore { get; set; } // 0-10

        [BsonElement("TransparencyPerformanceScore")]
        [JsonPropertyName("TransparencyPerformanceScore")]
        public double TransparencyPerformanceScore { get; set; } // 0-10

        [BsonElement("TransparencySustainabilityScore")]
        [JsonPropertyName("TransparencySustainabilityScore")]
        public double TransparencySustainabilityScore { get; set; } // 0-5

        [BsonElement("TransparencyTotalScore")]
        [JsonPropertyName("TransparencyTotalScore")]
        public string TransparencyTotalScore { get; set; } = string.Empty;

        [BsonElement("TransparencyScale")]
        [JsonPropertyName("TransparencyScale")]
        public int TransparencyScale { get; set; } // 1-5

        [BsonElement("TransparencyRating")]
        [JsonPropertyName("TransparencyRating")]
        public string TransparencyRating { get; set; } = string.Empty;
    }
}