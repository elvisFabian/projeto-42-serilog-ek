using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Destructurama.Attributed;
using Elastic.Kibana.Serilog.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Elastic.Kibana.Serilog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            _logger.LogInformation("Iniciando consulta de previsão do tempo para todas as cidades");

            var rng = new Random();
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();

            return result.AsHttpResponse();
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            using (_logger.BeginScope("ScopeId {scopeId}", Guid.NewGuid()))
            {
                _logger.LogInformation("Iniciando consulta de previsão do tempo para cidade {id}", id);

                var rng = new Random();
                var weatherForecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                    {
                        CidadeId = id,
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = rng.Next(-20, 55),
                        Summary = Summaries[rng.Next(Summaries.Length)]
                    })
                    .First();

                _logger.LogInformation("Resultado da previsao do tempo {@weatherForecast}", weatherForecast);


                return weatherForecast.AsHttpResponse();
            }
        }

        [HttpPost("erro")]
        public WeatherForecast LancaException(Erro erro)
        {
            throw new Exception(erro.Mensagem);
        }
    }

    public class Erro
    {
        public string Mensagem { get; set; }
    }
}