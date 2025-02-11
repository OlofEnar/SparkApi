using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SparkApi.Models.DTOs.ResponseDTO;
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
        
        [HttpGet("with-events")]
        public async Task<IActionResult> GetUsersWithEvents()
        {
            var users = await _userRepo.GetUsersWithEventsAsync();

            if (users == null)
            {
                return NotFound();
            }

            var userDtos = _mapper.Map<List<UserDto>>(users);
            return Ok(userDtos);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
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
        public async Task<IActionResult> GetEventsByUserId(string userId)
        {
            var userEvents = await _userRepo.GetEventsByUserIdAsync(userId);

            if (userEvents == null)
            {
                return NotFound();
            }

            var eventDtos = _mapper.Map<List<EventDto>>(userEvents);
            return Ok(eventDtos);
        }
    }
}
