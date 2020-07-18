using CrawlerEngine.Common;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using CrawlerEngine.Repository.Common.Interface;
using Dapper;
using System;
using System.Collections.Generic;

namespace CrawlerEngine.Repository.Crawl
{
    public class StockJobListRepository : BulkInsert<StockJobListDto>, IDisposable
    {
        private bool disposedValue = false;
        private IDatabaseConnectionHelper _DatabaseConnection;

        internal StockJobListRepository(IDatabaseConnectionHelper databaseConnectionHelper)
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

        ~StockJobListRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public IEnumerable<StockJobListDto> GetStockJobListDtos()
        {
            string sqlCommand = $@" select * from StockJobList";
            using (var conn = _DatabaseConnection.Create())
            {
                var result = conn.Query<StockJobListDto>(sqlCommand);
                return result;
            }
        }

        public int UpdateStatusEnd(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                    UPDATE [dbo].[StockJobList]
                                       SET  [JobStatus] = 'end'
                                           ,[EndTime] = N'{DateTime.UtcNow.ToString(RuleString.DateTimeFormat)}'
                                           ,[RegisterTime]= N'{DateTime.UtcNow.ToString(RuleString.DateTimeFormat)}'
                                   WHERE [Seq] = @Seq";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, jobInfo);
            }
        }
        public int UpdateJobStatusFail(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                    UPDATE [dbo].[StockJobList]
                                       SET  [JobStatus] = N'Fail'
                                           ,[EndTime] = N'{DateTime.UtcNow.ToString(RuleString.DateTimeFormat)}'
                                           ,[ErrorInfo]=N'{jobInfo.ErrorInfo}'
                                   WHERE [Seq] = @Seq";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, jobInfo);
            }
        }

        public int UpdateStatusStart(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                UPDATE [dbo].[StockJobList]
                                   SET [JobStatus] = N'start'
                                      ,[StartTime] = N'{DateTime.UtcNow.ToString(RuleString.DateTimeFormat)}'
                                 WHERE [Seq] = @Seq
                                ";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, jobInfo);
            }
        }
    }
}
