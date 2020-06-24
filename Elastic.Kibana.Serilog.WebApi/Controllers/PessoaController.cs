using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elastic.Kibana.Serilog.Dapper;
using Elastic.Kibana.Serilog.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Elastic.Kibana.Serilog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PessoaController : ControllerBase
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly ILogger<PessoaController> _logger;

        public PessoaController(IPessoaRepository pessoaRepository, ILogger<PessoaController> logger)
        {
            _pessoaRepository = pessoaRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _pessoaRepository.GetAll();

            return result.AsHttpResponse();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _pessoaRepository.Get(id);

            return result.AsHttpResponse();
        }

        [HttpGet("{nome:alpha}")]
        public async Task<IActionResult> GetByNome(string nome)
        {
            var result = await _pessoaRepository.Get(nome);

            return result.AsHttpResponse();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PessoaDto pessoa)
        {
            _logger.LogBodyInformation(pessoa);

            var result = await _pessoaRepository.Add(pessoa);

            return result.AsHttpResponse();
        }
    }
}