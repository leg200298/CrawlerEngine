using CrawlerEngine.Repository.Common.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CrawlerEngine.Repository.Common.Helper
{
    public class SensenDbConnectionHelper : IDatabaseConnectionHelper
    {
        private readonly string _connectionString;

        public SensenDbConnectionHelper()
        {
            this._connectionString = ConnectionString.SensenConnectionString;
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
