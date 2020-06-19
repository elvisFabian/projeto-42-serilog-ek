using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Serilog;

namespace System.Data.SqlClient
{
    public static class SqlConnectionExtensionsMethods
    {
        public static async Task<IEnumerable<T>> QueryAsync<T>(this SqlConnection connection, string sql, object parameters = null)
        {
            IEnumerable<T> records;

            connection.StatisticsEnabled = true;
            try
            {
                records = await SqlMapper.QueryAsync<T>(connection, sql, parameters);
            }
            catch (Exception originalException)
            {
                throw AddAdditionalInfoToException(originalException, $"Error: {nameof(QueryAsync)}: " + typeof(T).Name, sql, parameters);
            }

            var stats = connection.RetrieveStatistics();
            LogInfo("GetRecords: " + typeof(T).Name, stats, sql, parameters);

            return records;
        }

        public static async Task<T> QueryFirstOrDefaultAsync<T>(this SqlConnection connection, string sql, object parameters = null)
        {
            T record;

            connection.StatisticsEnabled = true;
            try
            {
                record = await SqlMapper.QueryFirstOrDefaultAsync<T>(connection, sql, parameters);
            }
            catch (Exception originalException)
            {
                throw AddAdditionalInfoToException(originalException, $"Error: {nameof(QueryFirstOrDefaultAsync)}: " + typeof(T).Name, sql, parameters);
            }

            var stats = connection.RetrieveStatistics();
            LogInfo("GetRecords: " + typeof(T).Name, stats, sql, parameters);

            return record;
        }

        private static void LogInfo(string logPrefix, IDictionary stats, string sql, object parameters = null)
        {
            var connectionTime = (long) stats["ConnectionTime"];

            Log
                .ForContext("sql_Script", sql)
                .ForContext("sql_Parameters", parameters)
                .ForContext("sql_ExecutionTime", stats["ExecutionTime"])
                .ForContext("sql_NetworkServerTime", stats["NetworkServerTime"])
                .ForContext("sql_BytesSent", stats["BytesSent"])
                .ForContext("sql_BytesReceived", stats["BytesReceived"])
                .ForContext("sql_SelectRows", stats["SelectRows"])
                .ForContext("sql_ConnectionTime", connectionTime)
                .Information("{logPrefix} in {ElaspedTime:0.0000} ms", logPrefix, connectionTime);
        }

        private static Exception AddAdditionalInfoToException(Exception originalException, string message, string sql, object parameters = null)
        {
            var additionalInfoException = new Exception(message, originalException);
            additionalInfoException.Data.Add("SQL", sql);
            var props = parameters?.GetType().GetProperties() ?? new PropertyInfo[] { };

            foreach (var prop in props)
            {
                additionalInfoException.Data.Add(prop.Name, prop.GetValue(parameters));
            }

            return additionalInfoException;
        }
    }
}