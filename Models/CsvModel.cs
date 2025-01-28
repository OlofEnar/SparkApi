using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SparkApi.Models
{
    public class CsvModel
    {
        [Name("User")]
        public string UserId { get; set; }

        [Name("Event date")]
        public DateOnly Date { get; set; }

        [Name("Event name")]
        public string? EventName { get; set; }

        [Name("Number of events")]
        public int EventCount { get; set; }
    }
}
