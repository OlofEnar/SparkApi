using AutoMapper;
using CsvHelper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SparkApi.Data;
using SparkApi.Models;
using SparkApi.Models.DbModels;
using System.Globalization;

namespace SparkApi.Services
{
    // Class for handling the csv import to db
    public class CSVService(IMapper mapper, AppDbContext dbContext)
    {
        public IEnumerable<T> ExtractCsv<T>(string filePath)
        {
            Log.Information("Fetching data from local csv...");
            var reader = new StreamReader(filePath);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<T>();
            return records;
        }

        public async Task ImportNewUserstoDb()
        {
            var data = ExtractCsv<CsvModel>("Data/export.csv"); // optimize this
            var userIds = data.Select(data => data.UserId).Distinct().ToList();
            int newUserCount = 0;

            foreach (var userId in userIds)
            {
                var existingUser = await dbContext.Users.FindAsync(userId);
                if (existingUser == null)
                {
                    var newUser = new User
                    {
                        UserId = userId,
                    };
                    newUserCount++;
                    await dbContext.Users.AddAsync(newUser);
                }
            }
            await dbContext.SaveChangesAsync();
            Log.Information("Total users in db: {users}", userIds.Count);
            Log.Information("New users added: {newUserCount}", newUserCount);
            newUserCount = 0;
        }
        public async Task ImportCsvDatatoDb()
        {
            var data = ExtractCsv<CsvModel>("Data/export.csv");
            await ImportNewUserstoDb(); //injecting data as param not working?
            
            var mappedData = mapper.Map<List<Event>>(data);
            var users = await dbContext.Users.ToListAsync();

            DateOnly date = new DateOnly(2024,10,8);
                //DateOnly.FromDateTime(DateTime.Today);

            foreach (var user in users)
            {
                var userEvents = mappedData.Where(e => e.UserId == user.UserId).ToList();
                user.TotalDailyEvents = userEvents.Count(e => e.Date == date);
                user.MostUsedDailyEvent = userEvents
                    .OrderByDescending(e => e.EventCount)
                    .FirstOrDefault().EventName;
            }

            try
            {
                await dbContext.Events.AddRangeAsync(mappedData);
                await dbContext.SaveChangesAsync();
                Log.Information("{mappedData} events imported to db", mappedData.Count);
            
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Unable to import data");            
            }
        }

        public void UpdateTotalUserEvents(List<User> users)
        {
            foreach (var user in users)
            {
                if (user.Events != null)
                { user.TotalEvents = user.Events.Count; }
                else { user.TotalEvents = 0; }
            }
            dbContext.SaveChangesAsync();
        }
    }
}