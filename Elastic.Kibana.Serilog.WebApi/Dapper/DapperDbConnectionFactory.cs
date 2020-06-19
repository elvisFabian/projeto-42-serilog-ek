using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Elastic.Kibana.Serilog.Dapper
{
    public class DapperDbConnectionFactory : IDbConnectionFactory
    {
        private readonly IDictionary<DatabaseConnectionName, string> _connectionDict;

        public DapperDbConnectionFactory(IDictionary<DatabaseConnectionName, string> connectionDict)
        {
            _connectionDict = connectionDict;
        }

        public SqlConnection CreateDbConnection(DatabaseConnectionName connectionName)
        {
            if (_connectionDict.TryGetValue(connectionName, out string connectionString))
            {
                return new SqlConnection(connectionString);
            }

            throw new ArgumentNullException();
        }
    }
}