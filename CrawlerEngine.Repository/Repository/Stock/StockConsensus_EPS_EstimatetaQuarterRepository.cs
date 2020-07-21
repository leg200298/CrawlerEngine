using CrawlerEngine.Model.DTO;
using CrawlerEngine.Repository.Common.Interface;
using Dapper;
using System;

namespace CrawlerEngine.Repository.Crawl
{
    public class StockConsensus_EPS_EstimatetaQuarterRepository : BulkInsert<StockConsensus_EPS_EstimatetaQuarterDto>, IDisposable
    {
        private bool disposedValue = false;
        private IDatabaseConnectionHelper _DatabaseConnection;

        internal StockConsensus_EPS_EstimatetaQuarterRepository(IDatabaseConnectionHelper databaseConnectionHelper)
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

        ~StockConsensus_EPS_EstimatetaQuarterRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public int InsertOne(StockConsensus_EPS_EstimatetaQuarterDto stockConsensus_EPS_EstimatetaQuarterDto)
        {
            string sqlCommand = $@"
INSERT INTO [dbo].[StockConsensus_EPS_EstimatetaQuarter]
           ([Date]
           ,[Code]
           ,[Legal_estimate_EPS]
           ,[Legal_estimate_EPS_unit]
           ,[Formula_estimate_EPS]
           ,[Formula_estimate_EPS_unit]
           ,[Legal_estimated_yield]
           ,[Legal_estimated_yield_unit]
           ,[Formula_estimated_yield]
           ,[Formula_estimated_yield_unit])
     VALUES
           (@Date
           ,@Code
           ,@Legal_estimate_EPS
           ,@Legal_estimate_EPS_unit
           ,@Formula_estimate_EPS
           ,@Formula_estimate_EPS_unit
           ,@Legal_estimated_yield
           ,@Legal_estimated_yield_unit
           ,@Formula_estimated_yield
           ,@Formula_estimated_yield_unit)


";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, stockConsensus_EPS_EstimatetaQuarterDto);
            }
        }
    }
}
