using AutoMapper;
using SparkApi.Data;
using SparkApi.Models;
using SparkApi.Models.DbModels;
using SparkApi.Utils;
using Microsoft.EntityFrameworkCore;

namespace SparkApi.Services
{    
    public class DbService(AppDbContext dbContext)
    {
        public async Task UpdateTotalUserEvents()
        {
            var users = await dbContext.Users.ToListAsync();

            foreach (var user in users)
            {
                if (user.Events != null)
                { user.TotalEvents = user.Events.Count; }
                else { user.TotalEvents = 0; }
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateUsersScore()
        {
            var users = await dbContext.Users.ToListAsync();

            foreach (var user in users)
            {
                var userEvents = await dbContext.Events
                    .Where(e => e.UserId == user.UserId)
                    .GroupBy(e => e.Date)
                    .Select(g => g.Sum(e => e.EventCount))                    
                    .ToListAsync();

                int eventTotal = userEvents.Sum();
                int dateCount = userEvents.Count();

                user.Score = ActivityIndex.CalcIndexWeighted(eventTotal, dateCount);
                Console.WriteLine("Weighted Index:");
                Console.Write($"date count: {dateCount}");
                Console.Write($"\tevent total: {eventTotal}");
                Console.Write($"\tScore: {user.Score}");
                Console.WriteLine(" ");

                user.Score = ActivityIndex.CalcIndexExpDecay(eventTotal, dateCount);
                Console.WriteLine("Exp Decay Index:");
                Console.Write($"date count: {dateCount}");
                Console.Write($"\tevent total: {eventTotal}");
                Console.Write($"\tScore: {user.Score}");
                Console.WriteLine(" ");

            }
            await dbContext.SaveChangesAsync();
        }
    }
}