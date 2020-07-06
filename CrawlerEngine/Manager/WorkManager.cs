using CrawlerEngine.Common.Helper;
using CrawlerEngine.Driver;
using CrawlerEngine.JobWorker;
using CrawlerEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrawlerEngine.Manager
{
    class WorkManager
    {
        private Condition resourseSetting;
        private List<string> mailTo;
        private int resourceCount;
        public void Process()
        {
            resourceCount = 1;
            WebDriverPool.InitDriver(resourceCount);
            while (1 == 1)
            {
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
                Task.Delay(10000);
            }
        }


        #region 工作區
        private IEnumerable<JobInfo> GetJobInfo()
        {
            return

              from x in Repository.Factory.CrawlFactory.CrawlDataJobListRepository.GetCrawlDataJobListDtos(resourceCount)
              select new JobInfo()
              {
                  Info = JsonUntityHelper.DeserializeStringToDictionary<string, object>(x.JobInfo),
                  Seq = x.Seq
              };
        }
        private List<JobInfo> Init()
        {

            throw new Exception("沒做");
        }
        private bool DoJob(JobInfo jobInfo)
        {
            var success = false;
            try
            {
                new JobWorkerFactory().GetJobWorker(jobInfo).DoJobFlow();
                success = true;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return success;

        }
        private bool SendErrorEmail()
        {
            foreach (var user in mailTo)
            {

                //send error mail

            }
            throw new Exception("沒做");

        }



        #endregion
    }
}
