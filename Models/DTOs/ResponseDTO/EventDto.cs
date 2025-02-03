using System.Text.Json.Serialization;

namespace SparkApi.Models.DTOs.ResponseDTO
{
    public class EventDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("eventName")]
        public string? EventName { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }
    }
}
