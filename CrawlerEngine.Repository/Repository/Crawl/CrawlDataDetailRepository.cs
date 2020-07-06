using CrawlerEngine.Model.DTO;
using CrawlerEngine.Repository.Common.Interface;
using Dapper;
using System;
using System.Collections.Generic;

namespace CrawlerEngine.Repository.Crawl
{
    public class CrawlDataDetailRepository : BulkInsert<CrawlDataDetailDto>, IDisposable
    {
        private bool disposedValue = false;
        private IDatabaseConnectionHelper _DatabaseConnection;

        internal CrawlDataDetailRepository(IDatabaseConnectionHelper databaseConnectionHelper)
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

        ~CrawlDataDetailRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public IEnumerable<CrawlDataDetailDto> GetDataDetailDtos()
        {
            string sqlCommand = @"SELECT *
                              FROM [dbo].[CrawlDataDetail] with(nolock)";
            using (var conn = _DatabaseConnection.Create())
            {
                var result = conn.Query<CrawlDataDetailDto>(sqlCommand);
                return result;
            }
        }

        public int InsertDataDetail(CrawlDataDetailDto crawlDataDetailDto)
        {
            string sqlCommand = $@"
                                    INSERT INTO[dbo].[CrawlDataDetail]
                                               ([Seq]
                                               ,[DetailData]
                                               ,[JobStatus]
                                               ,[EndTime])
                                         VALUES
                                               (@Seq
                                               ,@DetailData
                                               ,@JobStatus
                                               ,@EndTime)
                                    
                                    ";
            using (var conn = _DatabaseConnection.Create())
            {
                var result = conn.Execute(sqlCommand, crawlDataDetailDto);
                return result;
            }
        }
    }
}
