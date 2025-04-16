using SparkApi.Models.DbModels;
using SparkApi.Models.DTOs;

namespace SparkApi.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<Event?> GetEventByIdAsync(int id);
        Task<IEnumerable<Event>> GetEventsByNameAsync(string eventName);
        Task<AggregatedResponse> GetAggregatedEventsGroupedByDateAsync(DateOnly startDate, DateOnly endDate);
        Task<AggregatedResponse> GetAggregatedEventsGroupedByNameAsync(DateOnly startDate, DateOnly endDate);
        Task<AggregatedResponse> GetDailyAggregatedEventsForEventAsync(DateOnly startDate, DateOnly endDate, string eventName);
        Task<IEnumerable<AggregatedEventDetail>> GetAggregatedTimestampsByHourAsync(DateOnly startDate, DateOnly endDate, string eventName);
    }
}
