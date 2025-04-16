namespace SparkApi.Models.DTOs
{
    public class AggregatedResponse
    {
        public int? TotalEvents { get; set; }
        public int? TotalUsers { get; set; }
        public List<AggregatedEventData>? AggregatedEvents { get; set; }
        public List<AggregatedUserData>? AggregatedUsers { get; set; }
    }
}