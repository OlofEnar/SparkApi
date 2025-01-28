using System.ComponentModel.DataAnnotations;
namespace SparkApi.Models.DbModels
{
    public class User
    {
        [Key]
        public string UserId { get; set; }
        public List<string> ClientVersion { get; set; } = [];
        public List<string> UserCountry { get; set; } = [];
        public decimal? Score { get; set; }
        public int? TotalEvents { get; set; }
        public ICollection<Event>? Events { get; set; }
    }
}