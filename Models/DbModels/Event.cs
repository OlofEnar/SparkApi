using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SparkApi.Models.DbModels
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? EventName { get; set; }
        public string UserId { get; set; }
    }
}
