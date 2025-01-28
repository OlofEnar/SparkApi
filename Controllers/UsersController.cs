using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Core.Types;
using SparkApi.Data;
using SparkApi.Models.DbModels;
using SparkApi.Models.DTOs.ResponseDTO;
using System.Diagnostics.Tracing;


namespace SparkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(AppDbContext dbContext, IMapper mapper) : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await dbContext.Users.ToListAsync();

            if (users == null)
            {
                return NotFound();
            }

            var userDtos = mapper.Map<List<UserDto>>(users);

            return Ok(userDtos);
        }

        // GET: api/<UsersController>
        [HttpGet("AllUsersWithEvents")]
        public async Task<IActionResult> GetUsersWithEvents()
        {
            var users = await dbContext.Users
                .Include(u => u.Events)
                .ToListAsync();

            if (users == null)
            {
                return NotFound();
            }

            var userDtos = mapper.Map<List<UserDto>>(users);

            return Ok(userDtos);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await dbContext.Users
                .Include(u => u.Events)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = mapper.Map<UserDto>(user);

            return Ok(userDto);
        }



        // GET api/<UsersController>/5
        [HttpGet("{userId}/events")]
        public async Task<IActionResult> GetEventsByUserId(string userId)
        {
            var userEvents = await dbContext.Events
                .Where(e => e.UserId == userId)
            .ToListAsync();

            if (userEvents == null)
            {
                return NotFound($"No events found for user {userId}");
            }

            var eventDtos = mapper.Map<List<EventDto>>(userEvents);

            return Ok(eventDtos);
        }
    }
}
