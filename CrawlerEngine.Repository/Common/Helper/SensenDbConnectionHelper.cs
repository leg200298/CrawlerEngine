using CrawlerEngine.Repository.Common.Interface;
using System.Data;
using System.Data.SqlClient;

namespace CrawlerEngine.Repository.Common.Helper
{
    public class SensenDbConnectionHelper : IDatabaseConnectionHelper
    {
        private readonly string _connectionString;

        public SensenDbConnectionHelper()
        {
            _connectionString = ConnectionString.SensenConnectionString;
        }

        /// <summary>
        /// Create DbConnection
        /// </summary>
        /// <returns></returns>
        public IDbConnection Create()
        {
            var sqlConnection = new SqlConnection(_connectionString);
            return sqlConnection;
        }
    }
}
