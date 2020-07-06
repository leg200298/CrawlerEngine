using CrawlerEngine.JobWorker.Interface;
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
                #region Momo
                case "MOMO-PRODUCT":
                    return new WorkClass.Momo.ProductJobWorker(jobInfo);
                #endregion

                #region Pchome
                case "PCHOME-PRODUCT":
                    return new WorkClass.Pchome.ProductJobWorker(jobInfo);
                case "PCHOME-REGION":
                    return new WorkClass.Pchome.RegionJobWorker(jobInfo);
                case "PCHOME-STORE":
                    return new WorkClass.Pchome.StoreJobWorker(jobInfo);
                #endregion

                default:
                    return null;
                    break;
            }
        }
    }
}
