using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SparkApi.Models.DbModels
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public DateTime Date { get; set; }
        [MaxLength(50)]
        public string? EventName { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User? User { get; set; }
    }
}
