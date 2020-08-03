using CrawlerEngine.Common;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using CrawlerEngine.Repository.Common.Interface;
using CrawlerEngine.Repository.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrawlerEngine.Repository.PostgresSQL
{
    public class PostgresSQLCrawlDataJobListRepository : BulkInsert<CrawlDataJobListDto>, IDisposable, ICrawlDataJobListRepository
    {
        private bool disposedValue = false;
        private IDatabaseConnectionHelper _DatabaseConnection;

        internal PostgresSQLCrawlDataJobListRepository(IDatabaseConnectionHelper databaseConnectionHelper)
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

        ~PostgresSQLCrawlDataJobListRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public IEnumerable<CrawlDataJobListDto> GetCrawlDataJobListDtos(int resourceCount, string machineName)
        {
            string sqlCommand = $@"
                                     UPDATE crawl_data_job_list SET job_status='get',start_time = now(),work_station = '{machineName}'
 WHERE seq IN (
  SELECT seq FROM crawl_data_job_list WHERE job_status in ('not start' ,'waitforawhile')
  FETCH FIRST {resourceCount} ROWS ONLY)
 RETURNING *;";

            //sqlCommand = $@"SELECT seq, job_info, job_type, register_time, job_status, start_time, end_time, error_info
            //FROM public.crawl_data_job_list
            //where seq = 'e1c14e8b-b880-416a-91d8-92fc42d810fa'";
            using (var conn = _DatabaseConnection.Create())
            {
                var result = conn.Query<CrawlDataJobListDto>(sqlCommand);
                return result;
            }
        }

        public int UpdateStatusEnd(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                    UPDATE  crawl_data_job_list
                                       SET  job_status = 'end'
                                           ,end_time = '{DateTime.UtcNow.ToString(RuleString.DateTimeFormat)}'
                                   WHERE seq = @Seq";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, jobInfo);
            }
        }
        public int UpdateJobStatusFail(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                    UPDATE crawl_data_job_list
                                       SET  job_status= 'Fail'
                                           ,end_time = '{DateTime.UtcNow.ToString(RuleString.DateTimeFormat)}'
                                           ,error_info='{jobInfo.ErrorInfo}'
                                   WHERE seq = @Seq";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, jobInfo);
            }
        }

        public int UpdateStatusStart(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                UPDATE crawl_data_job_list
                                   SET job_status = 'start'
                                      ,start_time = '{DateTime.UtcNow.ToString(RuleString.DateTimeFormat)}'
                                 WHERE seq = @Seq
                                ";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, jobInfo);
            }
        }
        public int InsertOne(JobInfo jobInfo, string jobType)
        {
            string sqlCommand = $@"
                                    INSERT INTO crawl_data_job_list
                                               (seq,job_info,job_type)
                                         VALUES
                                               ('{Guid.NewGuid()}',N'{jobInfo.GetJsonString()}', N'{jobType}')


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
            BulkInsertRecords(ref CrawlDataJobListDtos, "crawl_data_job_list", _DatabaseConnection.Create().ConnectionString);


        }

        public int UpdateStatusNotStart(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                UPDATE crawl_data_job_list
                                   SET job_status = 'not start'
                                   WHERE seq = @Seq
                                ";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, jobInfo);
            }
        }

        public int UpdateStatusWaitForaWhile(JobInfo jobInfo)
        {
            string sqlCommand = $@"
                                UPDATE crawl_data_job_list
                                   SET job_status = 'waitforawhile'
                                      ,start_time = '{DateTime.UtcNow.ToString(RuleString.DateTimeFormat)}'
                                      ,work_times = work_times+1
                                 WHERE seq = @Seq
                                ";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, jobInfo);
            }
        }
    }
}
