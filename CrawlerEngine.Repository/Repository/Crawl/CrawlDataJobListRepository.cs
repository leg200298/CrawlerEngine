using CrawlerEngine.Repository.Common.Interface;
using CrawlerEngine.Repository.DTO;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.Repository.Crawl
{
  public  class CrawlDataJobListRepository
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
                              FROM [dbo].[CrawlDataJobList] with(nolock)";
            using (var conn = _DatabaseConnection.Create())
            {
                var result = conn.Query<CrawlDataJobListDto>(sqlCommand);
                return result;
            }
        }
    }
}
