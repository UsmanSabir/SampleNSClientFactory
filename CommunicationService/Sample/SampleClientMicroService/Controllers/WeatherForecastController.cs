using System.Diagnostics;
using CommunicationServiceAbstraction;
using Microsoft.AspNetCore.Mvc;
using Sample.Contracts;

namespace SampleClientMicroService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IClientFactory _clientFactory;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var businessPartnerService = _clientFactory.CreateClient<IBusinessPartnerService>();
            var bpModel = businessPartnerService.GetById(1);
            Debug.WriteLine(bpModel.Id + " : " + bpModel.Name);
            bpModel.Name = "abc";
            bpModel = businessPartnerService.UpdateModel(3, "Temp", bpModel);
            Debug.WriteLine(bpModel.Id + " : " + bpModel.Name);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}