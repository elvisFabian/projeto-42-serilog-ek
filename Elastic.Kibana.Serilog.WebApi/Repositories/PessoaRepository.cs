using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Elastic.Kibana.Serilog.Dto;

namespace Elastic.Kibana.Serilog.Dapper
{
    public class PessoaRepository : DbConnection1RepositoryBase, IPessoaRepository
    {
        public PessoaRepository(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory, DatabaseConnectionName.Projeto42)
        {
        }

        public async Task<IEnumerable<PessoaDto>> GetAll()
        {
            const string sql = "select * FROM Pessoa as P";

            await using var connection = CreateDbConnection();

            var result = await connection.QueryAsync<PessoaDto>(sql);
            return result;
        }

        public async Task<PessoaDto> Get(int id)
        {
            const string sql = "select * FROM Pessoas as P";

            await using var connection = CreateDbConnection();

            var result = await connection.QueryFirstOrDefaultAsync<PessoaDto>(sql);
            return result;
        }
    }
}