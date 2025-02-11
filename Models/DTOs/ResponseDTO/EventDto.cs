using System.Text.Json.Serialization;

namespace SparkApi.Models.DTOs.ResponseDTO
{
    public class EventDto
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        public string? EventName { get; set; }

        public int EventCount { get; set; }
        public List<EventDetail>? EventDetails { get; set; }

        public string UserId { get; set; }
    }
}
