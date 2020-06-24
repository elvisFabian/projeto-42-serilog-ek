using System.Collections.Generic;
using System.Linq;
using Elastic.Kibana.Serilog.EF;
using Elastic.Kibana.Serilog.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elastic.Kibana.Serilog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CidadeController : ControllerBase
    {
        private readonly Projeto42DbContext _projeto42DbContext;

        public CidadeController(Projeto42DbContext projeto42DbContext)
        {
            _projeto42DbContext = projeto42DbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _projeto42DbContext.Cidades.OrderBy(x => x.Uf).ThenBy(x => x.Nome).ToList();

            return result.AsHttpResponse();
        }

        [HttpGet("{uf:alpha}")]
        public IActionResult GetByUf(string uf)
        {
            var result = _projeto42DbContext.Cidades.Where(x => x.Uf.Equals(uf)).OrderBy(x => x.Nome).ToList();

            return result.AsHttpResponse();
        }
    }
}