using SparkApi.Models.DbModels;
using SparkApi.Models.DTOs.ResponseDTO;

namespace SparkApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(string id);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<IEnumerable<User>> GetUsersWithEventsAsync();
        Task<IEnumerable<Event>> GetEventsByUserIdAsync(string userId);

    }
}
