using CrawlerEngine.JobWorker.Interface;
using CrawlerEngine.JobWorker.WorkClass;
using CrawlerEngine.Models;

namespace CrawlerEngine.JobWorker
{
    public class JobWorkerFactory
    {
        public IJobWorker GetJobWorker(JobInfo jobInfo)
        {
            var jobWorker = jobInfo.Info["url"].ToString().ToLower().IndexOf("pchome") > 1 ? "PCHOME" : "MOMO";

            switch (jobWorker)
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
