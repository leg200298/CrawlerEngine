using CrawlerEngine.Driver;
using CrawlerEngine.Driver.WorkClass;
using CrawlerEngine.JobWorker;
using CrawlerEngine.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrawlerEngine.Manager
{
    class WorkManager
    {
        private Condition resourseSetting;
        private List<string> mailTo;
        public void Process()
        {
            WebDriverPool.InitDriver(3);
            try
            {

                Parallel.ForEach(GetJobInfo(), jobInfo =>
                {
                    DoJob(jobInfo);
                });
            }
            catch (Exception e)
            {
                SendErrorEmail();
            }
          //
            // throw new Exception("沒做");
        }
        public List<JobInfo> GetJobInfo()
        {

            throw new Exception("沒做");
        }
        public List<JobInfo> Init()
        {

            throw new Exception("沒做");
        }
        public bool DoJob(JobInfo jobInfo)
        {
            var success = false;
            try
            {
                new JobWorkerFactory().GetJobWorker(jobInfo).DoJobFlow();
                success= true;
            }
            catch (Exception) {
              
            }
            return success;

        }
        public bool SendErrorEmail()
        {
            foreach (var user in mailTo)
            {

                //send error mail

            }
            throw new Exception("沒做");

        }
    }
}
