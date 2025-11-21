using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System; // make sure this is at the top

namespace finance_management_backend.Models
{
    public class GrcExceptionLog
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

        [BsonElement("GRC Adequacy")]
        [JsonPropertyName("GRC Adequacy")]
        public string GrcAdequacy { get; set; } = string.Empty;

        [BsonElement("GRC Effectiveness")]
        [JsonPropertyName("GRC Effectiveness")]
        public string GrcEffectiveness { get; set; } = string.Empty;

        [BsonElement("Explanation")]
        [JsonPropertyName("Explanation")]
        public string Explanation { get; set; } = string.Empty;
    }
}
