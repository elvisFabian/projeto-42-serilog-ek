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

        [HttpGet("{uf:alpha}")]
        public IEnumerable<Cidade> GetAll(string uf)
        {
            return _projeto42DbContext.Cidades.Where(x => x.Uf.Equals(uf)).ToList();
        }
    }
}