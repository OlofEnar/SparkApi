using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SparkApi.Models.DTOs;
using SparkApi.Repositories.Interfaces;


namespace SparkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }
        

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepo.GetUsersAsync();

            if (users == null)
            {
                return NotFound();
            }

            var userDtos = _mapper.Map<List<UserDto>>(users);
            return Ok(userDtos);
        }

        [HttpGet("with-aggregated-events")]
        public async Task<IActionResult> GetUsersWithAggregatedEvents(DateOnly startDate, DateOnly endDate)
        {
            var users = await _userRepo.GetUsersWithAggregatedEventsAsync(startDate, endDate);

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpGet("{userId}/events")]
        public async Task<IActionResult> GetEventsByUserId(Guid userId)
        {
            var userEvents = await _userRepo.GetEventsByUserIdAsync(userId);

            if (userEvents == null)
            {
                return NotFound();
            }

            var eventDtos = _mapper.Map<List<EventDto>>(userEvents);
            return Ok(eventDtos);
        }


        [HttpGet("{userId}/aggregated-events-by-name")]
        public async Task<IActionResult> GetAggregatedEventsByUserIdGroupedByName(DateOnly startDate, DateOnly endDate, Guid userId)
        {
            var userEvents = await _userRepo.GetAggregatedEventsByUserIdGroupedByNameAsync(startDate, endDate, userId);

            if (userEvents == null)
            {
                return NotFound();
            }

            return Ok(userEvents);
        }

        [HttpGet("{userId}/aggregated-events-by-date")]
        public async Task<IActionResult> GetAggregatedEventsByUserIdGroupedByDate(DateOnly startDate, DateOnly endDate, Guid userId)
        {
            var userEvents = await _userRepo.GetAggregatedEventsByUserIdGroupedByDateAsync(startDate, endDate, userId);

            if (userEvents == null)
            {
                return NotFound();
            }

            return Ok(userEvents);
        }
    }
}
