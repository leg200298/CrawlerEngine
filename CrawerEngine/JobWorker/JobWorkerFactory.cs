using CrawerEngine.JobWorker.Interface;
using CrawerEngine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.JobWorker
{
    class JobWorkerFactory
    {
        public IJobWorker GetJobWorker(JobInfo jobInfo) {
            throw new Exception();
        }
    }
}
