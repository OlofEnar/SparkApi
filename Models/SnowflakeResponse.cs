using SparkApi.Models.DbModels;

namespace SparkApi.Models
{
    public class SnowflakeResponse: Event
    {
        public string? ClientVersion { get; set; }
        public string? UserCountry { get; set; }
    }
}
