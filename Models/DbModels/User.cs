namespace SparkApi.Models.DbModels
{
    public class User
    {
        public required Guid Id { get; set; }
        public string? ClientVersion { get; set; }
        public string? UserCountry { get; set; }
        public List<Event>? Events { get; set; } = [];
    }
}