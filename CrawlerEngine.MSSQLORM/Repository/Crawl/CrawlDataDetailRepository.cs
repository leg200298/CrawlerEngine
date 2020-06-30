using CrawlerEngine.Repository.Common.Interface;
using CrawlerEngine.Repository.DTO;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.Repository.Crawl
{
   public class CrawlDataDetailRepository: IDisposable
    {
        private bool disposedValue = false;
        private IDatabaseConnectionHelper _DatabaseConnection;

        internal CrawlDataDetailRepository(IDatabaseConnectionHelper databaseConnectionHelper)
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
    }
}
