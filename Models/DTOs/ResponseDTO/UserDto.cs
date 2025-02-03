using SparkApi.Models.DbModels;
using System.Text.Json.Serialization;

namespace SparkApi.Models.DTOs.ResponseDTO
{
    public class UserDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("client version")]
        public List<string> ClientVersion { get; set; } = [];
        [JsonPropertyName("country")]
        public List<string> UserCountry { get; set; } = [];

        [JsonPropertyName("score")]
        public decimal? Score { get; set; }

        [JsonPropertyName("totalEvents")]
        public int? TotalEvents { get; set; }

        [JsonPropertyName("events")]
        public ICollection<EventDto>? EventsDtos { get; set; } = [];
    }
}
