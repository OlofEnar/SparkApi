using SparkApi.Models.DbModels;
using System.Text.Json.Serialization;

namespace SparkApi.Models.DTOs.ResponseDTO
{
    public class UserDto
    {
        [JsonPropertyName("id")]
        public string UserId { get; set; }

        [JsonPropertyName("score")]
        public decimal? Score { get; set; }

        [JsonPropertyName("totalEvents")]
        public int? TotalEvents { get; set; }

        [JsonPropertyName("events")]
        public ICollection<EventDto>? EventsDtos { get; set; } = [];
    }
}
