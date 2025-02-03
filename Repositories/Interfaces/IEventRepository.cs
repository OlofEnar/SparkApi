using SparkApi.Models.DbModels;
using SparkApi.Models.DTOs.ResponseDTO;

namespace SparkApi.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<Event?> GetEventByIdAsync(int id);
        Task<IEnumerable<Event>> GetEventsByNameAsync(string eventName);
        Task<IEnumerable<Event>> GetEventsByUserIdAsync(string userId);

        Task<IEnumerable<Event>> GetEventsAsync();
    }
}
