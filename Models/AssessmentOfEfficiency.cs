// Models/AssessmentOfEfficiency.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace finance_management_backend.Models
{
    public class AssessmentOfEfficiency
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

        [BsonElement("Date")]
        [JsonPropertyName("Date")]
        public DateTime Date { get; set; }

        [BsonElement("ObjectiveAchievementScore")]
        [JsonPropertyName("ObjectiveAchievementScore")]
        public double ObjectiveAchievementScore { get; set; } // 0-10

        [BsonElement("TimelinessThroughputScore")]
        [JsonPropertyName("TimelinessThroughputScore")]
        public double TimelinessThroughputScore { get; set; } // 0-10

        [BsonElement("ResourceConsumptionScore")]
        [JsonPropertyName("ResourceConsumptionScore")]
        public double ResourceConsumptionScore { get; set; } // 0-5

        [BsonElement("EfficiencyScore")]
        [JsonPropertyName("EfficiencyScore")]
        public double EfficiencyScore { get; set; } // 0-25

        [BsonElement("TotalScore")]
        [JsonPropertyName("TotalScore")]
        public double TotalScore { get; set; }

        [BsonElement("Scale")]
        [JsonPropertyName("Scale")]
        public int Scale { get; set; } // 1-5

        [BsonElement("Rating")]
        [JsonPropertyName("Rating")]
        public string Rating { get; set; } = string.Empty; // Now string
    }
}