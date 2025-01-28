using AutoMapper;
using SparkApi.Data;
using SparkApi.Models;
using SparkApi.Models.DbModels;
using SparkApi.Utils;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SparkApi.Services
{    
    public class DbService(IMapper mapper, AppDbContext dbContext)
    {
        public async Task ImportNewUserstoDb(List<User> newUsers)
        {
            if (newUsers != null)
            {
                await dbContext.Users.AddRangeAsync(newUsers);
                await dbContext.SaveChangesAsync();
                Log.Information("Total users in db: {users}", dbContext.Users.Count());
                Log.Information("New users added: {newUserCount}", newUsers.Count);
            }           
            else
            {
                Log.Error("No data to read");
            }
        }

        public async Task ImportEventstoDb(List<Event> events)
        {
            if (events != null)
            {
                await dbContext.Events.AddRangeAsync(events);
                await dbContext.SaveChangesAsync();
                Log.Information($"Success! {events.Count()} events added to db.");
            }
            else
            {
                Log.Error("No data to read");
            }
        }

        public async Task UpdateUsersScoreandTotalEvents()
        {
            var users = await dbContext.Users.ToListAsync();

            foreach (var user in users)
            {
                var userEvents = await dbContext.Events
                    .Where(e => e.UserId == user.UserId)
                    .ToListAsync();

                int eventTotal = userEvents.Count();
                int dateCount = userEvents
                    .Select(e => e.Date.Date)
                    .Distinct()
                    .Count();

                user.TotalEvents = eventTotal;
                user.Score = ActivityIndex.CalcIndexWeighted(eventTotal, dateCount);
                Console.Write($"\tID: {user.UserId}");
                Console.Write($"date count: {dateCount}");
                Console.Write($"\tevent total: {eventTotal}");
                Console.Write($"\tScore: {user.Score}");
                Console.WriteLine(" ");
            }
            await dbContext.SaveChangesAsync();
            Log.Information("Score & Total Events updated successfully");
        }

        public async Task<HashSet<string>> GetUserIds()
        {
            var userIds = await dbContext.Users.Select(u => u.UserId).ToHashSetAsync();
            return userIds;
        }
    }
}