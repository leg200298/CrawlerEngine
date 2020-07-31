using CrawlerEngine.Common;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Repository.Common.Interface;
using CrawlerEngine.Repository.Interface;
using Dapper;
using System;
using System.Collections.Generic;

namespace CrawlerEngine.Repository.PostgresSQL
{
    public class PostgresSQLCrawlDataDetailRepository : BulkInsert<CrawlDataDetailDto>, IDisposable, ICrawlDataDetailRepository
    {
        private bool disposedValue = false;
        private IDatabaseConnectionHelper _DatabaseConnection;

        internal PostgresSQLCrawlDataDetailRepository(IDatabaseConnectionHelper databaseConnectionHelper)
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

        ~PostgresSQLCrawlDataDetailRepository()
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
                                               ('{crawlDataDetailDto.seq}'
                                               ,N'{crawlDataDetailDto.detail_data}'
                                               ,N'{crawlDataDetailDto.job_status}'
                                               ,'{crawlDataDetailDto.end_time.Value.ToString(RuleString.DateTimeFormat)}')
                                    
                                    ";
            using (var conn = _DatabaseConnection.Create())
            {
                var result = conn.Execute(sqlCommand, crawlDataDetailDto);
                return result;
            }
        }
    }
}
