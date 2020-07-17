using CrawlerEngine.Common;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using CrawlerEngine.Repository.Common.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrawlerEngine.Repository.Crawl
{
    public class StockListRepository : BulkInsert<CrawlDataJobListDto>, IDisposable
    {
        private bool disposedValue = false;
        private IDatabaseConnectionHelper _DatabaseConnection;

        internal StockListRepository(IDatabaseConnectionHelper databaseConnectionHelper)
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

        ~StockListRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public IEnumerable<StockListDto> GetCrawlDataJobListDtos()
        {
            string sqlCommand = $@" select * from StockList";
            using (var conn = _DatabaseConnection.Create())
            {
                var result = conn.Query<StockListDto>(sqlCommand);
                return result;
            }
        }

        public int UpdateStatusEnd(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                    UPDATE [dbo].[StockList]
                                       SET  [JobStatus] = 'end'
                                           ,[EndTime] = '{DateTime.UtcNow.ToString(RuleString.DateTimeFormat)}'
                                   WHERE [Seq] = @Seq";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, jobInfo);
            }
        }
        public int UpdateJobStatusFail(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                    UPDATE [dbo].[StockList]
                                       SET  [JobStatus] = 'Fail'
                                           ,[EndTime] = '{DateTime.UtcNow.ToString(RuleString.DateTimeFormat)}'
                                           ,[ErrorInfo]='{jobInfo.ErrorInfo}'
                                   WHERE [Seq] = @Seq";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, jobInfo);
            }
        }

        public int UpdateStatusStart(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                UPDATE [dbo].[StockList]
                                   SET [JobStatus] = 'start'
                                      ,[StartTime] = '{DateTime.UtcNow.ToString(RuleString.DateTimeFormat)}'
                                 WHERE [Seq] = @Seq
                                ";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, jobInfo);
            }
        }
        public int InsertOne(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                    INSERT INTO [dbo].[StockList]
                                               ([JobInfo])
                                         VALUES
                                               (N'{jobInfo.GetJsonString()}')


                                ";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand);
            }
        }
        public void InsertMany(List<JobInfo> jobInfos)
        {
            var CrawlDataJobListDtos = (from a in jobInfos
                                        select new CrawlDataJobListDto
                                        {
                                            JobInfo = a.GetJsonString()
                                        }).ToList();
            BulkInsertRecords(ref CrawlDataJobListDtos, "StockList", _DatabaseConnection.Create().ConnectionString);


        }
    }
}
