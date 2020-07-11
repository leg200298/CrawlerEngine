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
    public class PublicToiletInfoRepository : BulkInsert<CrawlDataJobListDto>, IDisposable
    {
        private bool disposedValue = false;
        private IDatabaseConnectionHelper _DatabaseConnection;

        internal PublicToiletInfoRepository(IDatabaseConnectionHelper databaseConnectionHelper)
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

        ~PublicToiletInfoRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<PublicToiletInfoDto> GetPublicToiletInfoDtos()
        {
            string sqlCommand = @"SELECT *
                              FROM [dbo].[PublicToiletInfo] with(nolock)";
            using (var conn = _DatabaseConnection.Create())
            {
                var result = conn.Query<PublicToiletInfoDto>(sqlCommand);
                return result;
            }
        }

        public int InsertPublicToiletInfo(PublicToiletInfoDto  publicToiletInfoDto)
        {
            string sqlCommand = $@"
                                    INSERT INTO [dbo].[PublicToiletInfo]
                                               ([toilet_id]
                                               ,[toilet_name]
                                               ,[toilet_address]
                                               ,[toilet_cellphone]
                                               ,[toilet_longitude]
                                               ,[toilet_latitude])
                                         VALUES
                                               (@toilet_id
                                               ,@toilet_name
                                               ,@toilet_address
                                               ,@toilet_cellphone
                                               ,@toilet_longitude
                                               ,@toilet_latitude)


                                    
                                    ";
            using (var conn = _DatabaseConnection.Create())
            {
                var result = conn.Execute(sqlCommand, publicToiletInfoDto);
                return result;
            }
        }
    }
}
