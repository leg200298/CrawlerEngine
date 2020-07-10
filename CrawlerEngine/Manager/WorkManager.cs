using CrawlerEngine.Common.Helper;
using CrawlerEngine.Driver;
using CrawlerEngine.JobWorker;
using CrawlerEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CrawlerEngine.Manager
{
    class WorkManager
    {
        private Condition resourseSetting;
        private List<string> mailTo;
        public void Process(int resourceCount)
        {
            WebDriverPool.InitDriver(resourceCount);
            var freeDriverCount = resourceCount;
            while (1 == 1)
            {

                freeDriverCount = WebDriverPool.GetFreeDriverConut();
                try
                {

                    Parallel.ForEach(GetJobInfo(freeDriverCount), jobInfo =>
                    {
                        DoJob(jobInfo);
                    });
                }
                catch (Exception ex)
                {
                    SendErrorEmail();
                    LoggerHelper._.Error(ex);
                }
                Thread.Sleep(10000);
            }
        }


        #region 工作區
        private IEnumerable<JobInfo> GetJobInfo(int resourceCount)
        {
            return

              from x in Repository.Factory.CrawlFactory.CrawlDataJobListRepository.GetCrawlDataJobListDtos(resourceCount)
              select new JobInfo()
              {
                  Info = JsonUntityHelper.DeserializeStringToDictionary<string, object>(x.JobInfo),
                  Seq = x.Seq
              };
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
                LoggerHelper._.Error(ex);
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
