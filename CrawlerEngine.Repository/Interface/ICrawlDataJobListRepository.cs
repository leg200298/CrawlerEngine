using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using CrawlerEngine.Models.ViewModel;
using System.Collections.Generic;

namespace CrawlerEngine.Repository.Interface
{
    public interface ICrawlDataJobListRepository
    {
        IEnumerable<CrawlStatus> GetCrawlDataJobDiffStatus();
        IEnumerable<CrawlDataJobListDto> GetCrawlDataJobListDtos(int resourceCount, string machineName);
        IEnumerable<CrawlDataJobListDto> GetCrawlDataJobListDtos(string command, int count);
        int UpdateStatusEnd(JobInfo jobInfo);
        int UpdateJobStatusFail(JobInfo jobInfo);

        int UpdateStatusStart(JobInfo jobInfo);
        int UpdateStatusWaitForaWhile(JobInfo jobInfo);

        int UpdateStatusNotStart(JobInfo jobInfo);
        int InsertOne(JobInfo jobInfo, string jobType);
        void InsertMany(List<JobInfo> jobInfos);
    }
}
