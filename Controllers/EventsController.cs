using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SparkApi.Models.DTOs;
using SparkApi.Repositories.Interfaces;


namespace SparkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;

        public EventsController(IEventRepository eventRepo, IMapper mapper)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var userEvent = await _eventRepo.GetEventByIdAsync(id);

            if (userEvent == null)
            {
                return NotFound();
            }

            var eventDto = _mapper.Map<EventDto>(userEvent);
            return Ok(eventDto);
        }


        [HttpGet("events-by-date")]
        public async Task<IActionResult> GetAggregatedEventsGroupedByDate(DateOnly startDate, DateOnly endDate)
        {
            var userEvents = await _eventRepo.GetAggregatedEventsGroupedByDateAsync(startDate,endDate);

            if (userEvents == null)
            {
                return NotFound();
            }

            return Ok(userEvents);
        }

        [HttpGet("aggregate-events-by-names")]
        public async Task<IActionResult> GetAggregatedEventsGroupedByName(DateOnly startDate, DateOnly endDate)
        {
            var userEvents = await _eventRepo.GetAggregatedEventsGroupedByNameAsync(startDate, endDate);

            if (userEvents == null)
            {
                return NotFound();
            }

            return Ok(userEvents);
        }

        [HttpGet("aggregate-events-by-name")]
        public async Task<IActionResult> GetDailyAggregatedEventsForEvent(DateOnly startDate, DateOnly endDate, string eventName)
        {
            var userEvents = await _eventRepo.GetDailyAggregatedEventsForEventAsync(startDate, endDate, eventName);

            if (userEvents == null)
            {
                return NotFound();
            }

            return Ok(userEvents);
        }

        [HttpGet("aggregate-timestamps-by-hour")]
        public async Task<IActionResult> GetAggregatedTimestampsByHour(DateOnly startDate, DateOnly endDate, string eventName)
        {
            var timestamps = await _eventRepo.GetAggregatedTimestampsByHourAsync(startDate, endDate, eventName);

            if (timestamps == null)
            {
                return NotFound();
            }

            return Ok(timestamps);
        }
    }
}
