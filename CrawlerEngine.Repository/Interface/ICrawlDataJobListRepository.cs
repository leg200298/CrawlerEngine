using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using System.Collections.Generic;

namespace CrawlerEngine.Repository.Interface
{
    public interface ICrawlDataJobListRepository
    {
        IEnumerable<CrawlDataJobListDto> GetCrawlDataJobListDtos(int resourceCount);
        int UpdateStatusEnd(JobInfo jobInfo);
        int UpdateJobStatusFail(JobInfo jobInfo);

        int UpdateStatusStart(JobInfo jobInfo);
        int InsertOne(JobInfo jobInfo, string jobType);
        void InsertMany(List<JobInfo> jobInfos);
    }
}
