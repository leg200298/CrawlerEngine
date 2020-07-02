using CrawlerEngine.JobWorker.Interface;
using CrawlerEngine.JobWorker.WorkClass;
using CrawlerEngine.Models;

namespace CrawlerEngine.JobWorker
{
    public class JobWorkerFactory
    {
        public IJobWorker GetJobWorker(JobInfo jobInfo)
        {
            var jobType = jobInfo.JobType;

            switch (jobType)
            {
                case "MOMO":
                    return new MomoJobWorker(jobInfo);
                case "PCHOME":
                default:
                    return new PchomeJobWorker(jobInfo);
            }
        }
    }
}
