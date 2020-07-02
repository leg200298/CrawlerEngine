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

            switch (jobType.ToUpper())
            {
                case "MOMO-PRODUCT":
                    return new MomoProductJobWorker(jobInfo);
                case "PCHOME-PRODUCT":
                default:
                    return new PchomeProductJobWorker(jobInfo);
            }
        }
    }
}
