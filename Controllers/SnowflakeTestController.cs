using Microsoft.AspNetCore.Mvc;
using SparkApi.Repositories;

namespace SparkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SnowflakeTestController: ControllerBase
    {
        private readonly SnowflakeRepository _snowRepo;

        public SnowflakeTestController(SnowflakeRepository snowRepo)
        {
            _snowRepo = snowRepo;
        }
    }
}
