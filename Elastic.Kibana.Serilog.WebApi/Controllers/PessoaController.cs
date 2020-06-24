using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elastic.Kibana.Serilog.Dapper;
using Elastic.Kibana.Serilog.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elastic.Kibana.Serilog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PessoaController : ControllerBase
    {
        private readonly IPessoaRepository _pessoaRepository;

        public PessoaController(IPessoaRepository pessoaRepository)
        {
            _pessoaRepository = pessoaRepository;
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
    }
}