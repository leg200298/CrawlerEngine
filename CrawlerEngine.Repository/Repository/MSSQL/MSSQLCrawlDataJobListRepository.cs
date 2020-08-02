using CrawlerEngine.Common;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using CrawlerEngine.Repository.Common.Interface;
using CrawlerEngine.Repository.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrawlerEngine.Repository.MSSQL
{
    public class MSSQLCrawlDataJobListRepository : BulkInsert<CrawlDataJobListDto>, IDisposable, ICrawlDataJobListRepository
    {
        private bool disposedValue = false;
        private IDatabaseConnectionHelper _DatabaseConnection;

        internal MSSQLCrawlDataJobListRepository(IDatabaseConnectionHelper databaseConnectionHelper)
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

        ~MSSQLCrawlDataJobListRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public IEnumerable<CrawlDataJobListDto> GetCrawlDataJobListDtos(int resourceCount)
        {
            string sqlCommand = $@"
                                    BEGIN TRAN
                                    UPDATE TOP({resourceCount}) crawl_data_job_list
                                    SET job_status='get'
                                        ,[start_time] = '{DateTime.UtcNow.ToString(RuleString.DateTimeFormat)}'
                                    OUTPUT inserted.*
                                    where  job_status in ('not start' ,'waitforawhile')                                  
                                    COMMIT TRAN";
            using (var conn = _DatabaseConnection.Create())
            {
                var result = conn.Query<CrawlDataJobListDto>(sqlCommand);
                return result;
            }
        }

        public int UpdateStatusEnd(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                    UPDATE [dbo].[crawl_data_job_list]
                                       SET  [job_status] = 'end'
                                           ,[end_time] = '{DateTime.UtcNow.ToString(RuleString.DateTimeFormat)}'
                                   WHERE [seq] = @seq";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, jobInfo);
            }
        }
        public int UpdateJobStatusFail(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                    UPDATE [dbo].[crawl_data_job_list]
                                       SET  [job_status] = 'Fail'
                                           ,[end_time] = '{DateTime.UtcNow.ToString(RuleString.DateTimeFormat)}'
                                           ,[error_info]='{jobInfo.ErrorInfo}'
                                   WHERE [seq] = @seq";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, jobInfo);
            }
        }

        public int UpdateStatusStart(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                UPDATE [dbo].[crawl_data_job_list]
                                   SET [job_status] = 'start'
                                      ,[start_time] = '{DateTime.UtcNow.ToString(RuleString.DateTimeFormat)}'
                                 WHERE [seq] = @seq
                                ";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, jobInfo);
            }
        }
        public int InsertOne(JobInfo jobInfo, string jobType)
        {
            string sqlCommand = $@"
                                    INSERT INTO [dbo].[crawl_data_job_list]
                                               ([job_info],[job_type])
                                         VALUES
                                               (N'{jobInfo.GetJsonString()}', N'{jobType}')


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
                                            job_info = a.GetJsonString()
                                        }).ToList();
            BulkInsertRecords(ref CrawlDataJobListDtos, "CrawlDataJobList", _DatabaseConnection.Create().ConnectionString);


        }

        public int UpdateStatusNotStart(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                UPDATE [dbo].[crawl_data_job_list]
                                   SET [job_status] = 'not start'
                                 WHERE [seq] = @seq
                                ";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, jobInfo);
            }
        }

        public int UpdateStatusWaitForaWhile(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                UPDATE [dbo].[crawl_data_job_list]
                                   SET [job_status] = 'waitforawhile'
                                      ,[start_time] = '{DateTime.UtcNow.ToString(RuleString.DateTimeFormat)}'
                                 WHERE [seq] = @seq
                                ";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, jobInfo);
            }
        }
    }
}
