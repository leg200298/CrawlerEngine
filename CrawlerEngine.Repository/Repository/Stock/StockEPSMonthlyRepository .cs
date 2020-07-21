using CrawlerEngine.Model.DTO;
using CrawlerEngine.Repository.Common.Interface;
using Dapper;
using System;

namespace CrawlerEngine.Repository.Crawl
{
    public class StockEPSMonthlyRepository : BulkInsert<StockEPSMonthlyDto>, IDisposable
    {
        private bool disposedValue = false;
        private IDatabaseConnectionHelper _DatabaseConnection;

        internal StockEPSMonthlyRepository(IDatabaseConnectionHelper databaseConnectionHelper)
        {
            _DatabaseConnection = databaseConnectionHelper;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    return;
                }

                disposedValue = true;
            }
        }

        ~StockEPSMonthlyRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public int InsertOne(StockEPSMonthlyDto stockEPSMonthlyDto)
        {
            string sqlCommand = $@"
INSERT INTO [dbo].[StockEPSMonthly]
           ([Date]
           ,[Code]
           ,[Year]
           ,[EPS_estimate_All_AVG_byMonth_latestonly]
           ,[EPS_estimate_All_AVG_byMonth_latestonly_unit])
     VALUES
           (@Date
           ,@Code
           ,@Year
           ,@EPS_estimate_All_AVG_byMonth_latestonly
           ,@EPS_estimate_All_AVG_byMonth_latestonly_unit)

";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, stockEPSMonthlyDto);
            }
        }
    }
}
