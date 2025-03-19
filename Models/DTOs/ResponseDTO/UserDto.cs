using System.Text.Json.Serialization;

namespace SparkApi.Models.DTOs.ResponseDTO
{
    public class UserDto
    {
        public required string Id { get; set; }
        public string? ClientVersion { get; set; }
        public string? UserCountry { get; set; }

        [JsonPropertyName("events")]
        public ICollection<EventDto>? EventDtos { get; set; } = [];
    }
}
