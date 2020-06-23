using CrawlerEngine.Driver;
using CrawlerEngine.Driver.WorkClass;
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
            Console.WriteLine("Process");
            try
            {
                Parallel.ForEach(GetJobInfo(), item=>
                {

                    DoJob(item);
                });
            }
            catch (Exception e)
            {
                SendErrorEmail();
            }
            WebDriverPool.DriverPool.Add(new SeleniumDriver());
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
            throw new Exception("沒做");

        }
        public bool SendErrorEmail()
        {
            foreach (var user in mailTo) {

                //send error mail

            }
            throw new Exception("沒做");

        }
    }
}
