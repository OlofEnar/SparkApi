using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SparkApi.Models.DTOs.ResponseDTO;
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

        [HttpGet]
        public async Task<IActionResult> Events()
        {
            var events = await _eventRepo.GetEventsAsync();

            if (events == null)
            {
                return NotFound();
            }

            var eventDtos = _mapper.Map<List<EventDto>>(events);
            return Ok(eventDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEventsById(int id)
        {
            var userEvent = await _eventRepo.GetEventByIdAsync(id);

            if (userEvent == null)
            {
                return NotFound();
            }

            var eventDto = _mapper.Map<EventDto>(userEvent);
            return Ok(eventDto);
        }

        [HttpGet("{eventName}")]
        public async Task<IActionResult> GetEventsByName(string eventName)
        {
            var userEvents = await _eventRepo.GetEventsByNameAsync(eventName);

            if (userEvents == null)
            {
                return NotFound();
            }

            var eventDtos = _mapper.Map<List<EventDto>>(userEvents);
            return Ok(eventDtos);
        }
    }
}
