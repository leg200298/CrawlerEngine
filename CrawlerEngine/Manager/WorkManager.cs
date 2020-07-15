using CrawlerEngine.Common.Helper;
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
        public void Process(int resourceCount)
        {
            //WebDriverPool.InitDriver(resourceCount);
            var freeDriverCount = resourceCount;
            //  while (1 == 1)
            {

                //freeDriverCount = WebDriverPool.GetFreeDriverConut();
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
               // int sleepTime = 24 * 60 * 60 * 1000;  //24小時 60分 60秒 1000毫秒
                //Thread.Sleep(sleepTime);
            }
        }


        #region 工作區
        private IEnumerable<JobInfo> GetJobInfo(int resourceCount)
        {
            List<JobInfo> lj = new List<JobInfo>();
            lj.Add(new JobInfo()
            {
                Seq = new Guid("D608BE51-D170-4056-ADD4-A54EA20DC1C4"),
                Info = JsonUntityHelper.DeserializeStringToDictionary<string, object>(
                    "{\"_url\": \"https://rate.bot.com.tw/gold?Lang=zh-TW\",   \"_jobType\": \"BANK-GOLD\"}")
            });
            lj.Add(new JobInfo()
            {
                Seq = new Guid("D608BE51-D170-4056-ADD4-A54EA20DC1C4"),
                Info = JsonUntityHelper.DeserializeStringToDictionary<string, object>(
                    "{\"_url\": \"https://rate.bot.com.tw/xrt?Lang=zh-TW\",   \"_jobType\": \"BANK-EXCHANGE\"}")
            });
            return lj.AsEnumerable();
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


            }
            throw new Exception("沒做");

        }



        #endregion
    }
}
