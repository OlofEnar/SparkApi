using SparkApi.Models.DbModels;

namespace SparkApi.Models.DTOs
{
    public class AggregatedUserData
    {
        public User User { get; set; }
        public Guid UserId { get; set; }
        public int EventTotal { get; set; }
        public double EventDistribution { get; set; }
    }
}