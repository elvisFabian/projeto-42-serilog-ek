using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
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
            const string sql = "select top 1000 * FROM Pessoa as P";

            await using var connection = CreateDbConnection();

            var result = await connection.QueryAsync<PessoaDto>(sql);
            return result;
        }

        public async Task<PessoaDto> Get(int id)
        {
            const string sql = "select * FROM Pessoa as P where id = @id ";

            await using var connection = CreateDbConnection();

            var result = await connection.QueryFirstOrDefaultAsync<PessoaDto>(sql, new {id});
            return result;
        }

        public async Task<PessoaDto> Get(string nome)
        {
            const string sql = "select * FROM Pessoas as P where nome = @nome ";

            await using var connection = CreateDbConnection();

            var result = await connection.QueryFirstOrDefaultAsync<PessoaDto>(sql, new {nome});
            return result;
        }

        public async Task<bool> Add(PessoaDto pessoa)
        {
            const string sql = "insert into Pessoa (Nome, Cpf) VALUES (@nome, @cpf)";

            await using var connection = CreateDbConnection();

            var result = await connection.ExecuteAsync(sql, pessoa);
            return result > 0;
        }
    }
}