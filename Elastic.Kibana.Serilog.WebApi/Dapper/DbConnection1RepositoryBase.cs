using System.Data.SqlClient;

namespace Elastic.Kibana.Serilog.Dapper
{
    public abstract class DbConnection1RepositoryBase
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly DatabaseConnectionName _connectionName;

        protected DbConnection1RepositoryBase(IDbConnectionFactory dbConnectionFactory, DatabaseConnectionName connectionName)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _connectionName = connectionName;
        }

        protected SqlConnection CreateDbConnection()
        {
            return _dbConnectionFactory.CreateDbConnection(_connectionName);
        }
    }
}