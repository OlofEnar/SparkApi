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

        [HttpGet("get-all")]
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
    }
}
