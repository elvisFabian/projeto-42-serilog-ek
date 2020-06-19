using System.Collections.Generic;
using System.Threading.Tasks;
using Elastic.Kibana.Serilog.Dto;

namespace Elastic.Kibana.Serilog.Dapper
{
    public interface IPessoaRepository
    {
        Task<IEnumerable<PessoaDto>> GetAll();
        Task<PessoaDto> Get(int id);
    }
}