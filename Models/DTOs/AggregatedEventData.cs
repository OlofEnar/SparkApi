namespace SparkApi.Models.DTOs
{
    public class AggregatedEventData
    {
        public string? EventName { get; set; }
        public DateOnly? EventDate { get; set; }
        public int EventTotal { get; set; }
        public int UniqueUsers { get; set; }
        public double EventDistribution { get; set; }
    }
}
