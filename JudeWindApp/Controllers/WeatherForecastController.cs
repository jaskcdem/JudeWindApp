using JudeWind.Model.Weather;
using JudeWindApp.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JudeWindApp.Controllers
{
    /// <summary> sample controller </summary>
    [AllowAnonymous]
    public class WeatherForecastController : BaseApiController
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        private readonly ILogger<WeatherForecastController> _logger;

        /// <summary> sample controller </summary>
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary> sample Controller method </summary>
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

#if DEBUG
        /// <summary> Download Img sample </summary>
        [HttpGet]
        [ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [BypassApiResult]
        public IActionResult DownloadImg() => DownloadPhysicalFile(@"C:\Projects\download\2024-09-18\IMG_2047.jpg");
#endif
    }
}
