using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System;

namespace finance_management_backend.Models
{
    public class OtherControlEnvironmentScoring
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
        // 1. Responsibility Delegation Matrix
        [BsonElement("ResponsibilityDelegationMatrix")]
        [JsonPropertyName("ResponsibilityDelegationMatrix")]
        public string ResponsibilityDelegationMatrix { get; set; } = string.Empty;

        [BsonElement("RdmDesignScore")]
        [JsonPropertyName("RdmDesignScore")]
        public double RdmDesignScore { get; set; } // 0-10

        [BsonElement("RdmPerformanceScore")]
        [JsonPropertyName("RdmPerformanceScore")]
        public double RdmPerformanceScore { get; set; } // 0-10

        [BsonElement("RdmSustainabilityScore")]
        [JsonPropertyName("RdmSustainabilityScore")]
        public double RdmSustainabilityScore { get; set; } // 0-5

        [BsonElement("RdmTotalScore")]
        [JsonPropertyName("RdmTotalScore")]
        public string RdmTotalScore { get; set; } = string.Empty;

        [BsonElement("RdmScale")]
        [JsonPropertyName("RdmScale")]
        public int RdmScale { get; set; } // 1-5

        [BsonElement("RdmRating")]
        [JsonPropertyName("RdmRating")]
        public string RdmRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 2. Segregation of Duties
        [BsonElement("SegregationOfDuties")]
        [JsonPropertyName("SegregationOfDuties")]
        public string SegregationOfDuties { get; set; } = string.Empty;

        [BsonElement("SodDesignScore")]
        [JsonPropertyName("SodDesignScore")]
        public double SodDesignScore { get; set; } // 0-10

        [BsonElement("SodPerformanceScore")]
        [JsonPropertyName("SodPerformanceScore")]
        public double SodPerformanceScore { get; set; } // 0-10

        [BsonElement("SodSustainabilityScore")]
        [JsonPropertyName("SodSustainabilityScore")]
        public double SodSustainabilityScore { get; set; } // 0-5

        [BsonElement("SodTotalScore")]
        [JsonPropertyName("SodTotalScore")]
        public string SodTotalScore { get; set; } = string.Empty;

        [BsonElement("SodScale")]
        [JsonPropertyName("SodScale")]
        public int SodScale { get; set; } // 1-5

        [BsonElement("SodRating")]
        [JsonPropertyName("SodRating")]
        public string SodRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 3. Reporting Lines
        [BsonElement("ReportingLines")]
        [JsonPropertyName("ReportingLines")]
        public string ReportingLines { get; set; } = string.Empty;

        [BsonElement("ReportingLinesDesignScore")]
        [JsonPropertyName("ReportingLinesDesignScore")]
        public double ReportingLinesDesignScore { get; set; } // 0-10

        [BsonElement("ReportingLinesPerformanceScore")]
        [JsonPropertyName("ReportingLinesPerformanceScore")]
        public double ReportingLinesPerformanceScore { get; set; } // 0-10

        [BsonElement("ReportingLinesSustainabilityScore")]
        [JsonPropertyName("ReportingLinesSustainabilityScore")]
        public double ReportingLinesSustainabilityScore { get; set; } // 0-5

        [BsonElement("ReportingLinesTotalScore")]
        [JsonPropertyName("ReportingLinesTotalScore")]
        public string ReportingLinesTotalScore { get; set; } = string.Empty;

        [BsonElement("ReportingLinesScale")]
        [JsonPropertyName("ReportingLinesScale")]
        public int ReportingLinesScale { get; set; } // 1-5

        [BsonElement("ReportingLinesRating")]
        [JsonPropertyName("ReportingLinesRating")]
        public string ReportingLinesRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 4. Mission
        [BsonElement("Mission")]
        [JsonPropertyName("Mission")]
        public string Mission { get; set; } = string.Empty;

        [BsonElement("MissionDesignScore")]
        [JsonPropertyName("MissionDesignScore")]
        public double MissionDesignScore { get; set; } // 0-10

        [BsonElement("MissionPerformanceScore")]
        [JsonPropertyName("MissionPerformanceScore")]
        public double MissionPerformanceScore { get; set; } // 0-10

        [BsonElement("MissionSustainabilityScore")]
        [JsonPropertyName("MissionSustainabilityScore")]
        public double MissionSustainabilityScore { get; set; } // 0-5

        [BsonElement("MissionTotalScore")]
        [JsonPropertyName("MissionTotalScore")]
        public string MissionTotalScore { get; set; } = string.Empty;

        [BsonElement("MissionScale")]
        [JsonPropertyName("MissionScale")]
        public int MissionScale { get; set; } // 1-5

        [BsonElement("MissionRating")]
        [JsonPropertyName("MissionRating")]
        public string MissionRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 5. Vision and Values
        [BsonElement("VisionAndValues")]
        [JsonPropertyName("VisionAndValues")]
        public string VisionAndValues { get; set; } = string.Empty;

        [BsonElement("VisionValuesDesignScore")]
        [JsonPropertyName("VisionValuesDesignScore")]
        public double VisionValuesDesignScore { get; set; } // 0-10

        [BsonElement("VisionValuesPerformanceScore")]
        [JsonPropertyName("VisionValuesPerformanceScore")]
        public double VisionValuesPerformanceScore { get; set; } // 0-10

        [BsonElement("VisionValuesSustainabilityScore")]
        [JsonPropertyName("VisionValuesSustainabilityScore")]
        public double VisionValuesSustainabilityScore { get; set; } // 0-5

        [BsonElement("VisionValuesTotalScore")]
        [JsonPropertyName("VisionValuesTotalScore")]
        public string VisionValuesTotalScore { get; set; } = string.Empty;

        [BsonElement("VisionValuesScale")]
        [JsonPropertyName("VisionValuesScale")]
        public int VisionValuesScale { get; set; } // 1-5

        [BsonElement("VisionValuesRating")]
        [JsonPropertyName("VisionValuesRating")]
        public string VisionValuesRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 6. Goals and Objectives
        [BsonElement("GoalsAndObjectives")]
        [JsonPropertyName("GoalsAndObjectives")]
        public string GoalsAndObjectives { get; set; } = string.Empty;

        [BsonElement("GoalsObjectivesDesignScore")]
        [JsonPropertyName("GoalsObjectivesDesignScore")]
        public double GoalsObjectivesDesignScore { get; set; } // 0-10

        [BsonElement("GoalsObjectivesPerformanceScore")]
        [JsonPropertyName("GoalsObjectivesPerformanceScore")]
        public double GoalsObjectivesPerformanceScore { get; set; } // 0-10

        [BsonElement("GoalsObjectivesSustainabilityScore")]
        [JsonPropertyName("GoalsObjectivesSustainabilityScore")]
        public double GoalsObjectivesSustainabilityScore { get; set; } // 0-5

        [BsonElement("GoalsObjectivesTotalScore")]
        [JsonPropertyName("GoalsObjectivesTotalScore")]
        public string GoalsObjectivesTotalScore { get; set; } = string.Empty;

        [BsonElement("GoalsObjectivesScale")]
        [JsonPropertyName("GoalsObjectivesScale")]
        public int GoalsObjectivesScale { get; set; } // 1-5

        [BsonElement("GoalsObjectivesRating")]
        [JsonPropertyName("GoalsObjectivesRating")]
        public string GoalsObjectivesRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 7. Structures & Systems
        [BsonElement("StructuresAndSystems")]
        [JsonPropertyName("StructuresAndSystems")]
        public string StructuresAndSystems { get; set; } = string.Empty;

        [BsonElement("StructuresSystemsDesignScore")]
        [JsonPropertyName("StructuresSystemsDesignScore")]
        public double StructuresSystemsDesignScore { get; set; } // 0-10

        [BsonElement("StructuresSystemsPerformanceScore")]
        [JsonPropertyName("StructuresSystemsPerformanceScore")]
        public double StructuresSystemsPerformanceScore { get; set; } // 0-10

        [BsonElement("StructuresSystemsSustainabilityScore")]
        [JsonPropertyName("StructuresSystemsSustainabilityScore")]
        public double StructuresSystemsSustainabilityScore { get; set; } // 0-5

        [BsonElement("StructuresSystemsTotalScore")]
        [JsonPropertyName("StructuresSystemsTotalScore")]
        public string StructuresSystemsTotalScore { get; set; } = string.Empty;

        [BsonElement("StructuresSystemsScale")]
        [JsonPropertyName("StructuresSystemsScale")]
        public int StructuresSystemsScale { get; set; } // 1-5

        [BsonElement("StructuresSystemsRating")]
        [JsonPropertyName("StructuresSystemsRating")]
        public string StructuresSystemsRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 8. Policies and Procedures
        [BsonElement("PoliciesAndProcedures")]
        [JsonPropertyName("PoliciesAndProcedures")]
        public string PoliciesAndProcedures { get; set; } = string.Empty;

        [BsonElement("PoliciesProceduresDesignScore")]
        [JsonPropertyName("PoliciesProceduresDesignScore")]
        public double PoliciesProceduresDesignScore { get; set; } // 0-10

        [BsonElement("PoliciesProceduresPerformanceScore")]
        [JsonPropertyName("PoliciesProceduresPerformanceScore")]
        public double PoliciesProceduresPerformanceScore { get; set; } // 0-10

        [BsonElement("PoliciesProceduresSustainabilityScore")]
        [JsonPropertyName("PoliciesProceduresSustainabilityScore")]
        public double PoliciesProceduresSustainabilityScore { get; set; } // 0-5

        [BsonElement("PoliciesProceduresTotalScore")]
        [JsonPropertyName("PoliciesProceduresTotalScore")]
        public string PoliciesProceduresTotalScore { get; set; } = string.Empty;

        [BsonElement("PoliciesProceduresScale")]
        [JsonPropertyName("PoliciesProceduresScale")]
        public int PoliciesProceduresScale { get; set; } // 1-5

        [BsonElement("PoliciesProceduresRating")]
        [JsonPropertyName("PoliciesProceduresRating")]
        public string PoliciesProceduresRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 9. Processes
        [BsonElement("Processes")]
        [JsonPropertyName("Processes")]
        public string Processes { get; set; } = string.Empty;

        [BsonElement("ProcessesDesignScore")]
        [JsonPropertyName("ProcessesDesignScore")]
        public double ProcessesDesignScore { get; set; } // 0-10

        [BsonElement("ProcessesPerformanceScore")]
        [JsonPropertyName("ProcessesPerformanceScore")]
        public double ProcessesPerformanceScore { get; set; } // 0-10

        [BsonElement("ProcessesSustainabilityScore")]
        [JsonPropertyName("ProcessesSustainabilityScore")]
        public double ProcessesSustainabilityScore { get; set; } // 0-5

        [BsonElement("ProcessesTotalScore")]
        [JsonPropertyName("ProcessesTotalScore")]
        public string ProcessesTotalScore { get; set; } = string.Empty;

        [BsonElement("ProcessesScale")]
        [JsonPropertyName("ProcessesScale")]
        public int ProcessesScale { get; set; } // 1-5

        [BsonElement("ProcessesRating")]
        [JsonPropertyName("ProcessesRating")]
        public string ProcessesRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 10. Integrity and Ethical Values
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
        // 11. Oversight Structure
        [BsonElement("OversightStructure")]
        [JsonPropertyName("OversightStructure")]
        public string OversightStructure { get; set; } = string.Empty;

        [BsonElement("OversightDesignScore")]
        [JsonPropertyName("OversightDesignScore")]
        public double OversightDesignScore { get; set; } // 0-10

        [BsonElement("OversightPerformanceScore")]
        [JsonPropertyName("OversightPerformanceScore")]
        public double OversightPerformanceScore { get; set; } // 0-10

        [BsonElement("OversightSustainabilityScore")]
        [JsonPropertyName("OversightSustainabilityScore")]
        public double OversightSustainabilityScore { get; set; } // 0-5

        [BsonElement("OversightTotalScore")]
        [JsonPropertyName("OversightTotalScore")]
        public string OversightTotalScore { get; set; } = string.Empty;

        [BsonElement("OversightScale")]
        [JsonPropertyName("OversightScale")]
        public int OversightScale { get; set; } // 1-5

        [BsonElement("OversightRating")]
        [JsonPropertyName("OversightRating")]
        public string OversightRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 12. Standards
        [BsonElement("Standards")]
        [JsonPropertyName("Standards")]
        public string Standards { get; set; } = string.Empty;

        [BsonElement("StandardsDesignScore")]
        [JsonPropertyName("StandardsDesignScore")]
        public double StandardsDesignScore { get; set; } // 0-10

        [BsonElement("StandardsPerformanceScore")]
        [JsonPropertyName("StandardsPerformanceScore")]
        public double StandardsPerformanceScore { get; set; } // 0-10

        [BsonElement("StandardsSustainabilityScore")]
        [JsonPropertyName("StandardsSustainabilityScore")]
        public double StandardsSustainabilityScore { get; set; } // 0-5

        [BsonElement("StandardsTotalScore")]
        [JsonPropertyName("StandardsTotalScore")]
        public string StandardsTotalScore { get; set; } = string.Empty;

        [BsonElement("StandardsScale")]
        [JsonPropertyName("StandardsScale")]
        public int StandardsScale { get; set; } // 1-5

        [BsonElement("StandardsRating")]
        [JsonPropertyName("StandardsRating")]
        public string StandardsRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 13. Methodologies
        [BsonElement("Methodologies")]
        [JsonPropertyName("Methodologies")]
        public string Methodologies { get; set; } = string.Empty;

        [BsonElement("MethodologiesDesignScore")]
        [JsonPropertyName("MethodologiesDesignScore")]
        public double MethodologiesDesignScore { get; set; } // 0-10

        [BsonElement("MethodologiesPerformanceScore")]
        [JsonPropertyName("MethodologiesPerformanceScore")]
        public double MethodologiesPerformanceScore { get; set; } // 0-10

        [BsonElement("MethodologiesSustainabilityScore")]
        [JsonPropertyName("MethodologiesSustainabilityScore")]
        public double MethodologiesSustainabilityScore { get; set; } // 0-5

        [BsonElement("MethodologiesTotalScore")]
        [JsonPropertyName("MethodologiesTotalScore")]
        public string MethodologiesTotalScore { get; set; } = string.Empty;

        [BsonElement("MethodologiesScale")]
        [JsonPropertyName("MethodologiesScale")]
        public int MethodologiesScale { get; set; } // 1-5

        [BsonElement("MethodologiesRating")]
        [JsonPropertyName("MethodologiesRating")]
        public string MethodologiesRating { get; set; } = string.Empty;

        // ───────────────────────────────────────────────
        // 14. Rules and Regulations
        [BsonElement("RulesAndRegulations")]
        [JsonPropertyName("RulesAndRegulations")]
        public string RulesAndRegulations { get; set; } = string.Empty;

        [BsonElement("RulesRegsDesignScore")]
        [JsonPropertyName("RulesRegsDesignScore")]
        public double RulesRegsDesignScore { get; set; } // 0-10

        [BsonElement("RulesRegsPerformanceScore")]
        [JsonPropertyName("RulesRegsPerformanceScore")]
        public double RulesRegsPerformanceScore { get; set; } // 0-10

        [BsonElement("RulesRegsSustainabilityScore")]
        [JsonPropertyName("RulesRegsSustainabilityScore")]
        public double RulesRegsSustainabilityScore { get; set; } // 0-5

        [BsonElement("RulesRegsTotalScore")]
        [JsonPropertyName("RulesRegsTotalScore")]
        public string RulesRegsTotalScore { get; set; } = string.Empty;

        [BsonElement("RulesRegsScale")]
        [JsonPropertyName("RulesRegsScale")]
        public int RulesRegsScale { get; set; } // 1-5

        [BsonElement("RulesRegsRating")]
        [JsonPropertyName("RulesRegsRating")]
        public string RulesRegsRating { get; set; } = string.Empty;
    }
}