using System.Text.Json.Serialization;

namespace SparkApi.Models.DTOs.ResponseDTO
{
    public class EventDto
    {
        [JsonPropertyName("id")]
        public int EventId { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("eventName")]
        public string? EventName { get; set; }

        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }
    }
}
