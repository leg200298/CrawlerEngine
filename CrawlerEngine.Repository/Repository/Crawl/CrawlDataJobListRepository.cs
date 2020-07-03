using CrawlerEngine.Model.DTO;
using CrawlerEngine.Repository.Common.Interface;
using Dapper;
using System;
using System.Collections.Generic;

namespace CrawlerEngine.Repository.Crawl
{
    public class CrawlDataJobListRepository
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

        public int UpdateStatusEnd(CrawlDataJobListDto crawlDataJobListDto)
        {
            string sqlCommand = $@"
                                    UPDATE [dbo].[CrawlDataJobList]
                                       SET  [JobStatus] = 'end'
                                           ,[EndTime] = {DateTime.Now}
                                   WHERE [Seq] = @Seq";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, crawlDataJobListDto);
            }
        }
        public int UpdateStatusStart(CrawlDataJobListDto crawlDataJobListDto)
        {
            string sqlCommand = $@"
                                UPDATE [dbo].[CrawlDataJobList]
                                   SET [JobStatus] = 'start'
                                      ,[StartTime] = {DateTime.Now}
                                 WHERE [Seq] = @Seq
                                ";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, crawlDataJobListDto);
            }
        }
    }
}
