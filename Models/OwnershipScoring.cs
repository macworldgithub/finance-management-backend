using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System;

namespace finance_management_backend.Models
{
    public class OwnershipScoring
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("Id")]
        public string? Id { get; set; }

        [BsonElement("Date")]
        [JsonPropertyName("Date")]
        public DateTime Date { get; set; }

        // The key field – usually comes from Ownership / Process No
        [BsonElement("No")]
        [JsonPropertyName("No")]
        public double No { get; set; }

        // ───────────────────────────────────────────────
        // Activity level
        [BsonElement("Activity")]
        [JsonPropertyName("Activity")]
        public string Activity { get; set; } = string.Empty;

        [BsonElement("ActivityScore")]
        [JsonPropertyName("ActivityScore")]
        public double ActivityScore { get; set; } // 0–25

        // ───────────────────────────────────────────────
        // Process level
        [BsonElement("Process")]
        [JsonPropertyName("Process")]
        public string Process { get; set; } = string.Empty;

        [BsonElement("ProcessScore")]
        [JsonPropertyName("ProcessScore")]
        public double ProcessScore { get; set; } // 0–25

        // ───────────────────────────────────────────────
        // Process Stage level
        [BsonElement("ProcessStage")]
        [JsonPropertyName("ProcessStage")]
        public string ProcessStage { get; set; } = string.Empty;

        [BsonElement("ProcessStageScore")]
        [JsonPropertyName("ProcessStageScore")]
        public double ProcessStageScore { get; set; } // 0–25

        // ───────────────────────────────────────────────
        // Aggregated / Total
        [BsonElement("TotalScore")]
        [JsonPropertyName("TotalScore")]
        public string TotalScore { get; set; } = string.Empty;     // ← string as requested

        [BsonElement("Scale")]
        [JsonPropertyName("Scale")]
        public int Scale { get; set; }                             // 1–5 usually

        [BsonElement("Rating")]
        [JsonPropertyName("Rating")]
        public string Rating { get; set; } = string.Empty;         // ← free text / string

        // ───────────────────────────────────────────────
        // Function / Organizational dimensions
        [BsonElement("Function")]
        [JsonPropertyName("Function")]
        public string Function { get; set; } = string.Empty;

        [BsonElement("FunctionScore")]
        [JsonPropertyName("FunctionScore")]
        public double FunctionScore { get; set; } // 0–25

        [BsonElement("ClientSegmentOrFunctionalSegment")]
        [JsonPropertyName("ClientSegmentAndOrFunctionalSegment")]
        public string ClientSegmentOrFunctionalSegment { get; set; } = string.Empty;

        [BsonElement("ClientSegmentScore")]
        [JsonPropertyName("ClientSegmentScore")]
        public double ClientSegmentScore { get; set; } // 0–25

        [BsonElement("OperationalUnit")]
        [JsonPropertyName("OperationalUnit")]
        public string OperationalUnit { get; set; } = string.Empty;

        [BsonElement("OperationalUnitScore")]
        [JsonPropertyName("OperationalUnitScore")]
        public double OperationalUnitScore { get; set; } // 0–25

        [BsonElement("Division")]
        [JsonPropertyName("Division")]
        public string Division { get; set; } = string.Empty;

        [BsonElement("DivisionScore")]
        [JsonPropertyName("DivisionScore")]
        public double DivisionScore { get; set; } // 0–25

        [BsonElement("Entity")]
        [JsonPropertyName("Entity")]
        public string Entity { get; set; } = string.Empty;

        [BsonElement("EntityScore")]
        [JsonPropertyName("EntityScore")]
        public double EntityScore { get; set; } // 0–25

        [BsonElement("UnitOrDepartment")]
        [JsonPropertyName("UnitOrDepartment")]
        public string UnitOrDepartment { get; set; } = string.Empty;

        [BsonElement("UnitOrDepartmentScore")]
        [JsonPropertyName("UnitOrDepartmentScore")]
        public double UnitOrDepartmentScore { get; set; } // 0–25

        [BsonElement("ProductClass")]
        [JsonPropertyName("ProductClass")]
        public string ProductClass { get; set; } = string.Empty;

        [BsonElement("ProductClassScore")]
        [JsonPropertyName("ProductClassScore")]
        public double ProductClassScore { get; set; } // 0–25

        [BsonElement("ProductName")]
        [JsonPropertyName("ProductName")]
        public string ProductName { get; set; } = string.Empty;

        [BsonElement("ProductNameScore")]
        [JsonPropertyName("ProductNameScore")]
        public double ProductNameScore { get; set; } // 0–25
    }
}