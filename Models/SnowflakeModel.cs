namespace SparkApi.Models
{
    public class SnowflakeModel
    {
        public string UserId { get; set; }
        public DateTime EventDate { get; set; }
        public string EventName { get; set; }

        public string ClientVersion { get; set; }
        public string UserCountry { get; set; }
    }
}
