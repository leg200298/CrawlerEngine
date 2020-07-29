using CrawlerEngine.Model.DTO;
using CrawlerEngine.Repository.Common.Interface;
using CrawlerEngine.Repository.Interface;
using Dapper;
using System;
using System.Collections.Generic;

namespace CrawlerEngine.Repository.MSSQL
{
    public class MSSQLCrawlDataDetailRepository : BulkInsert<CrawlDataDetailDto>, IDisposable, ICrawlDataDetailRepository
    {
        private bool disposedValue = false;
        private IDatabaseConnectionHelper _DatabaseConnection;

        internal MSSQLCrawlDataDetailRepository(IDatabaseConnectionHelper databaseConnectionHelper)
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

        ~MSSQLCrawlDataDetailRepository()
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
                              FROM crawl_data_detail with(nolock)";
            using (var conn = _DatabaseConnection.Create())
            {
                var result = conn.Query<CrawlDataDetailDto>(sqlCommand);
                return result;
            }
        }

        public int InsertDataDetail(CrawlDataDetailDto crawlDataDetailDto)
        {
            string sqlCommand = $@"
                                    INSERT INTO crawl_data_detail
                                               (seq
                                               ,detail_data
                                               ,job_status
                                               ,end_time)
                                         VALUES
                                               (@seq
                                               ,@detail_data
                                               ,@job_status
                                               ,@end_time)
                                    
                                    ";
            using (var conn = _DatabaseConnection.Create())
            {
                var result = conn.Execute(sqlCommand);
                return result;
            }
        }
    }
}
