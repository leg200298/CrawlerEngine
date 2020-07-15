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
    public class BankInfoRepository : BulkInsert<BankDto>, IDisposable
    {
        private bool disposedValue = false;
        private IDatabaseConnectionHelper _DatabaseConnection;

        internal BankInfoRepository(IDatabaseConnectionHelper databaseConnectionHelper)
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

        ~BankInfoRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public IEnumerable<BankDto> GetCrawlDataJobListDtos(int resourceCount)
        {
            string sqlCommand = $@"
                                    BEGIN TRAN
                                                          UPDATE TOP({resourceCount}) CrawlDataJobList
                                                          SET JobStatus='get'
                                                             ,[StartTime] = '{DateTime.UtcNow.ToString(RuleString.DateTimeFormat)}'
                                                          OUTPUT inserted.*
                                                          where JobStatus ='not start'
                                    COMMIT TRAN";
            using (var conn = _DatabaseConnection.Create())
            {
                var result = conn.Query<BankDto>(sqlCommand);
                return result;
            }
        }

        public int UpdateStatusEnd(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                    UPDATE [dbo].[CrawlDataJobList]
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
                                    UPDATE [dbo].[CrawlDataJobList]
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
                                UPDATE [dbo].[CrawlDataJobList]
                                   SET [JobStatus] = 'start'
                                      ,[StartTime] = '{DateTime.UtcNow.ToString(RuleString.DateTimeFormat)}'
                                 WHERE [Seq] = @Seq
                                ";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, jobInfo);
            }
        }
        public int InsertOne(BankDto bankDto)
        {
            string sqlCommand = $@"
                                    INSERT INTO [dbo].[BankInfo]
                                               ([Date],
                                                [Currency],
                                                [RateSell],
                                                [RateBuy])
                                         VALUES
                                               (@Date,
                                                @Currency,
                                                @RateSell,
                                                @RateBuy)


                                ";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, bankDto);
            }
        }
        public void InsertMany(List<JobInfo> jobInfos)
        {
            var CrawlDataJobListDtos = (from a in jobInfos
                                        select new BankDto
                                        {

                                        }).ToList();
            BulkInsertRecords(ref CrawlDataJobListDtos, "CrawlDataJobList", _DatabaseConnection.Create().ConnectionString);


        }
    }
}
