using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Snowflake.Data.Client;
using SparkApi.Repositories;
using SparkApi.Services;

namespace SparkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SnowflakeController : ControllerBase
    {
        private readonly SnowflakeService _snowService;

        public SnowflakeController(SnowflakeService snowService)
        {
            _snowService = snowService;
        }

        [HttpGet("ImportSnowflakeData")]
        public async Task<IActionResult> ImportSnowflakeData()
        {
            try
            {
                await _snowService.ProcessSnowflakeDataAsync();
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
