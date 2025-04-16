using SparkApi.Models.DbModels;
using SparkApi.Models.DTOs;

namespace SparkApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(Guid id);
        Task<IEnumerable<User>> GetUsersAsync();
        //Task<IEnumerable<User>> GetUsersWithEventsAsync();
        Task<AggregatedResponse> GetUsersWithAggregatedEventsAsync(DateOnly startDate, DateOnly endDate);
        Task<IEnumerable<Event>> GetEventsByUserIdAsync(Guid userId);
        Task<AggregatedResponse> GetAggregatedEventsByUserIdGroupedByNameAsync(DateOnly startDate, DateOnly endDate, Guid userId);
        Task<AggregatedResponse> GetAggregatedEventsByUserIdGroupedByDateAsync(DateOnly startDate, DateOnly endDate, Guid userId);

    }
}
