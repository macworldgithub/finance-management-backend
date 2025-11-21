using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System; // make sure this is at the top

namespace finance_management_backend.Models
{
    public class Ownership
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("Id")]
        public string? Id { get; set; }


            [BsonElement("Date")]
    [JsonPropertyName("Date")]
    public DateTime Date { get; set; }

        // No (key like 5.1)
        [BsonElement("No")]
        [JsonPropertyName("No")]
        public double No { get; set; }

        [BsonElement("Main Process")]
        [JsonPropertyName("Main Process")]
        public string MainProcess { get; set; } = string.Empty;

        [BsonElement("Activity")]
        [JsonPropertyName("Activity")]
        public string Activity { get; set; } = string.Empty;

        [BsonElement("Process")]
        [JsonPropertyName("Process")]
        public string Process { get; set; } = string.Empty;

        [BsonElement("Process Stage")]
        [JsonPropertyName("Process Stage")]
        public string ProcessStage { get; set; } = string.Empty;

        [BsonElement("Functions")]
        [JsonPropertyName("Functions")]
        public string Functions { get; set; } = string.Empty;

        [BsonElement("Client Segment and/or Functional Segment")]
        [JsonPropertyName("Client Segment and/or Functional Segment")]
        public string ClientSegmentOrFunctionalSegment { get; set; } = string.Empty;

        [BsonElement("Operational Unit")]
        [JsonPropertyName("Operational Unit")]
        public string OperationalUnit { get; set; } = string.Empty;

        [BsonElement("Division")]
        [JsonPropertyName("Division")]
        public string Division { get; set; } = string.Empty;

        [BsonElement("Entity")]
        [JsonPropertyName("Entity")]
        public string Entity { get; set; } = string.Empty;

        [BsonElement("Unit / Department")]
        [JsonPropertyName("Unit / Department")]
        public string UnitOrDepartment { get; set; } = string.Empty;

        [BsonElement("Product Class")]
        [JsonPropertyName("Product Class")]
        public string ProductClass { get; set; } = string.Empty;

        [BsonElement("Product Name")]
        [JsonPropertyName("Product Name")]
        public string ProductName { get; set; } = string.Empty;
    }
}
