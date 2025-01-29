using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Snowflake.Data.Client;
using SparkApi.Services;

namespace SparkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SnowflakeController(SnowflakeService snow, IMapper mapper) : ControllerBase
    {

        [HttpGet("ImportSnowflakeData")]
        public async Task<IActionResult> ImportSnowflakeData()
        {
            try
            {
                await snow.ProcessSnowflakeDataAsync();
                //var eventDtos = mapper.Map<List<EventDto>>(events);

                return Ok();
            }
            catch (SnowflakeDbException ex)
            {
                Log.Error($"Snowflake Error: {ex.SqlState}, {ex.Message}");
                return StatusCode(500, $"Snowflake Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Log.Error($"Unexpected Error: {ex.Message}");
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }

        }
    }

}
