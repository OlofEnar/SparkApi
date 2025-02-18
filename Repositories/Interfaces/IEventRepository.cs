using SparkApi.Models.DbModels;

namespace SparkApi.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<Event?> GetEventByIdAsync(int id);
        Task<IEnumerable<Event>> GetEventsByNameAsync(string eventName);
        Task<IEnumerable<Event>> GetEventsAsync();
    }
}
