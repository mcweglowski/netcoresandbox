using System.Collections.Generic;
using System.Linq;
using System;

namespace RestaurantAPI
{
    public class WeatherForecastService : IWeatherForecastService
    {
        public static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public IEnumerable<WeatherForecast> Get(int take, int minTemperature, int maxTermperature)
        {
            var rng = new Random();

            return Enumerable.Range(1, take).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(minTemperature, maxTermperature),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}