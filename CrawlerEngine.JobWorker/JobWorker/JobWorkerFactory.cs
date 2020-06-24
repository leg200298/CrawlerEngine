using CrawlerEngine.JobWorker.Interface;
using CrawlerEngine.JobWorker.WorkClass;
using CrawlerEngine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.JobWorker
{
    public class JobWorkerFactory
    {
        public IJobWorker GetJobWorker(JobInfo jobInfo) {
            switch (jobInfo.TargetType)
            {
                case 2:
                    return new  MomoJobWorker();
                case 1:
                default:
                    return new PchomeJobWorker();
            }
        }
    }
}
