using System;
using System.Collections.Generic;//
using System.Linq;//
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _service;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _service.Get(5, -20, 55);
        }

        [HttpGet("currentDay/{max}")]
        public IEnumerable<WeatherForecast> Get2([FromQuery]int take, [FromRoute]int max)
        {
            return _service.Get(take, -20, max);
        }

        [HttpPost]
        public string Hello([FromBody] string name)
        {
            return $"Hello {name}!";
        }

        [HttpPost("generate")]
        public ActionResult<IEnumerable<WeatherForecast>> Generate([FromQuery]int take, [FromBody]TemperatureRequest request)
        {
            if (take < 0 || request.Max < request.Min)
            {
                return BadRequest();
            }

            var result = _service.Get(take, request.Min, request.Max);

            return Ok(result);
        }
    }
}