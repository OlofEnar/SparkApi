namespace SparkApi.Models.DbModels
{
    public class User
    {
        public required string Id { get; set; }
        public string? ClientVersion { get; set; }
        public string? UserCountry { get; set; }
        public List<Event>? Events { get; set; } = [];
    }
}