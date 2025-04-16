namespace SparkApi.Models.DTOs
{
    public class EventDto
    {
        public int EventId { get; set; }

        public DateOnly EventDate { get; set; }

        public string? EventName { get; set; }

        public int EventCount { get; set; }
        public List<EventDetail>? EventDetails { get; set; }
        public required Guid UserId { get; set; }
    }
}
