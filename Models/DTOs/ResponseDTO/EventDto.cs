using SparkApi.Models.DbModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SparkApi.Models.DTOs.ResponseDTO
{
    public class EventDto
    {
        [JsonPropertyName("id")]
        public int EventId { get; set; }

        [JsonPropertyName("date")]
        public DateOnly Date { get; set; }

        [JsonPropertyName("eventName")]
        public string? EventName { get; set; }

        [JsonPropertyName("eventCount")]
        public int EventCount { get; set; }

        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }
    }
}
