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

        /// <summary> list Controller method </summary>
        [HttpGet(Name = "API")]
        public List<string> Api()
        {
            List<string> result = [];
            var baseType = typeof(ControllerBase);
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = Directory.GetFiles(path, "*.dll").Select(System.Reflection.Assembly.LoadFrom).ToArray();
            var controllers = referencedAssemblies.SelectMany(a => a.DefinedTypes).Select(ti => ti.AsType()).Where(t => t != baseType && !t.IsAbstract && baseType.IsAssignableFrom(t)).ToArray();
            Dictionary<Type, string> httpmethods = new()
            {
                { typeof(HttpGetAttribute), "GET"},
                { typeof(HttpPostAttribute), "POST"},
                { typeof(HttpPutAttribute), "PUT"},
                { typeof(HttpDeleteAttribute), "DELETE"},
                { typeof(HttpHeadAttribute), "HEAD"},
                { typeof(HttpPatchAttribute), "PATCH"},
                { typeof(HttpOptionsAttribute), "OPTIONS"},
            };
            foreach (var controller in controllers)
            {
                var members = controller.GetMembers().Where(m => m.MemberType == System.Reflection.MemberTypes.Method);
                if (controller.GetCustomAttributes(typeof(RouteAttribute), true) is not RouteAttribute?[] route || !route.Any(x => x != null)) continue;
                foreach (var member in members)
                {
                    foreach (var dic in httpmethods)
                    {
                        var attr = member.GetCustomAttributes(dic.Key, false);
                        if (attr != null && attr.Length > 0)
                        {
                            var url = GetUrl(route.Last()!, controller.Name, member.Name);
                            result.Add($"{dic.Value} \\{url}");
                            break;
                        }
                    }
                }
            }
            return result;
        }

#if DEBUG
        /// <summary> Download Img sample </summary>
        [HttpGet]
        [ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [BypassApiResult]
        public IActionResult DownloadImg() => DownloadPhysicalFile(@"C:\Projects\download\2024-09-18\IMG_2047.jpg");
#endif

        static string GetUrl(RouteAttribute route, string controllerName, string actionName) => route.Template
            .Replace("[controller]", controllerName, StringComparison.CurrentCultureIgnoreCase)
            .Replace("[action]", actionName, StringComparison.CurrentCultureIgnoreCase)
            .Replace("Controller", "", StringComparison.CurrentCultureIgnoreCase)
            .Replace("Async", "", StringComparison.CurrentCultureIgnoreCase)
            .Replace("/", "\\");
    }
}
