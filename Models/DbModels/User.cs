using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SparkApi.Data;

namespace SparkApi.Models.DbModels
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }
        public decimal? Score { get; set; }
        public int? TotalDailyEvents { get; set; }

        public string? MostUsedDailyEvent { get; set; }
        public int? TotalWeeklylEvents { get; set; }
        public int? TotalEvents { get; set; }

        public ICollection<Event>? Events { get; set; }

    }
}