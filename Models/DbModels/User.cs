namespace SparkApi.Models.DbModels
{
    public class User
    {
        public string Id { get; set; }
        public string[] ClientVersion { get; set; } = [];
        public string[] UserCountry { get; set; } = [];
        public decimal? Score { get; set; }
        public int? TotalEvents { get; set; }
        public List<Event>? Events { get; set; } = [];
    }
}