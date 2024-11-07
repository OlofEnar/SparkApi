using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SparkApi.Data;
using SparkApi.Models.DbModels;
using SparkApi.Models.DTOs.ResponseDTO;


namespace SparkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController(AppDbContext dbContext, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var events = await dbContext.Events
                .ToListAsync();

            if (events == null)
            {
                return NotFound();
            }

            var eventDtos = mapper.Map<List<EventDto>>(events);

            return Ok(eventDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userEvent = await dbContext.Events
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (userEvent == null)
            {
                return NotFound();
            }

            var eventDto = mapper.Map<EventDto>(userEvent);
            return Ok(eventDto);
        }
    }
}
