using System.Data;
using System.Data.SqlClient;

namespace Elastic.Kibana.Serilog.Dapper
{
    public interface IDbConnectionFactory
    {
        SqlConnection CreateDbConnection(DatabaseConnectionName connectionName);
    }
}