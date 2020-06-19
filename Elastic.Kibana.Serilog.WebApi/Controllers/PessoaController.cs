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
        public async Task<IEnumerable<PessoaDto>> GetAll()
        {
            return await _pessoaRepository.GetAll();
        }
        
        [HttpGet("{id:int}")]
        public async Task<PessoaDto> GetById(int id)
        {
            return await _pessoaRepository.Get(id);
        }
    }
}