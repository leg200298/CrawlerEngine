using CrawlerEngine.Common;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using CrawlerEngine.Repository.Common.Interface;
using Dapper;
using System;
using System.Collections.Generic;

namespace CrawlerEngine.Repository.Crawl
{
    public class CrawlDataJobListRepository : BulkInsert<CrawlDataJobListDto>, IDisposable
    {
        private bool disposedValue = false;
        private IDatabaseConnectionHelper _DatabaseConnection;

        internal CrawlDataJobListRepository(IDatabaseConnectionHelper databaseConnectionHelper)
        {
            this._DatabaseConnection = databaseConnectionHelper;
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

        ~CrawlDataJobListRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public IEnumerable<CrawlDataJobListDto> GetCrawlDataJobListDtos()
        {
            string sqlCommand = @"SELECT *
                              FROM [dbo].[CrawlDataJobList] with(nolock)
                              where JobStatus='not start'";
            using (var conn = _DatabaseConnection.Create())
            {
                var result = conn.Query<CrawlDataJobListDto>(sqlCommand);
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
        public int InsertOne(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                    INSERT INTO [dbo].[CrawlDataJobList]
                                               ([JobInfo])
                                         VALUES
                                               (N'{jobInfo.GetJsonString()}')


                                ";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand);
            }
        }
        public void InsertMany(List<JobInfo> jobInfos) {
            BulkInsertRecords(ref jobInfos,"", _DatabaseConnection.Create().ConnectionString);


        }
    }
}
