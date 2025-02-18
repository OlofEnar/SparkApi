using System.Text.Json.Serialization;

namespace SparkApi.Models.DTOs.ResponseDTO
{
    public class UserDto
    {
        public string Id { get; set; }
        public List<string> ClientVersion { get; set; } = [];
        public List<string> UserCountry { get; set; } = [];

        public decimal? Score { get; set; }

        public int? TotalEvents { get; set; }

        [JsonPropertyName("events")]
        public ICollection<EventDto>? EventDtos { get; set; } = [];
    }
}
