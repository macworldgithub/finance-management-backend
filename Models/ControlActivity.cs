using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System; // make sure this is at the top

namespace finance_management_backend.Models
{
    public class ControlActivity
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

        [BsonElement("Control Objectives")]
        [JsonPropertyName("Control Objectives")]
        public string ControlObjectives { get; set; } = string.Empty;

        [BsonElement("Control Ref")]
        [JsonPropertyName("Control Ref")]
        public string ControlRef { get; set; } = string.Empty;

        [BsonElement("Control Definition")]
        [JsonPropertyName("Control Definition")]
        public string ControlDefinition { get; set; } = string.Empty;

        [BsonElement("Control Description")]
        [JsonPropertyName("Control Description")]
        public string ControlDescription { get; set; } = string.Empty;

        [BsonElement("Control Responsibility")]
        [JsonPropertyName("Control Responsibility")]
        public string ControlResponsibility { get; set; } = string.Empty;

        [BsonElement("Key Control")]
        [JsonPropertyName("Key Control")]
        public string KeyControl { get; set; } = string.Empty;

        [BsonElement("Zero Tolerance")]
        [JsonPropertyName("Zero Tolerance")]
        public string ZeroTolerance { get; set; } = string.Empty;
    }
}
