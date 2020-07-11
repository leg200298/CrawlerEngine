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
#if (DEBUG)
            List<JobInfo> lj = new List<JobInfo>();
            lj.Add(new JobInfo() { Seq =new Guid("E82E3FA7-B58B-4F25-B827-DA73C8150F3C"), Info = JsonUntityHelper.DeserializeStringToDictionary<string, object>("  {  \"_jobType\": \"GoogleMap-Oil\",  \"_url\": \"https://www.google.com.tw/search?tbm=map&authuser=0&hl=zh-TW&gl=tw&q=全國深澳坑站\"}") });
            return lj.AsEnumerable();
#else
            return

              from x in Repository.Factory.CrawlFactory.CrawlDataJobListRepository.GetCrawlDataJobListDtos(resourceCount)
              select new JobInfo()
              {
                  Info = JsonUntityHelper.DeserializeStringToDictionary<string, object>(x.JobInfo),
                  Seq = x.Seq
              };

#endif
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
