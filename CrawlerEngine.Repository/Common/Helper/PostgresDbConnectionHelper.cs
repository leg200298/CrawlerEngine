using CrawlerEngine.Repository.Common.Interface;
using Npgsql;
using System.Data;

namespace CrawlerEngine.Repository.Common.Helper
{
    public class PostgresDbConnectionHelper : IDatabaseConnectionHelper
    {
        private readonly string _connectionString;

        public PostgresDbConnectionHelper()
        {
            _connectionString = ConnectionString.PostgresConnectionString;
        }

        /// <summary>
        /// Create DbConnection
        /// </summary>
        /// <returns></returns>
        public IDbConnection Create()
        {
            var sqlConnection = new NpgsqlConnection(_connectionString);
            return sqlConnection;
        }
    }
}
