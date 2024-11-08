using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SparkApi.Models.DbModels
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        public DateOnly Date { get; set; }

        public string? EventName { get; set; }

        public int EventCount { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
