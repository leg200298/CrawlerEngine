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
        public Repository.Factory.CrawlFactory CrawlFactory;
        public void Process(int resourceCount, string board, int start, int end)
        {
            CrawlFactory = new Repository.Factory.CrawlFactory("MSSQL");
            WebDriverPool.InitDriver(resourceCount);
            var freeDriverCount = resourceCount;

            try
            {

                Parallel.ForEach(GetJobInfo(freeDriverCount, board, start, end), jobInfo =>
                {
                    DoJob(jobInfo);
                });
            }
            catch (Exception ex)
            {
                //   SendErrorEmail();
                LoggerHelper._.Error(ex);
            }

            //Thread.Sleep(10000);
        }


        #region 工作區
        private IEnumerable<JobInfo> GetJobInfo(int resourceCount, string board, int start, int end)
        {
            //#if (DEBUG)
            List<JobInfo> lj = new List<JobInfo>();
            lj.Add(new JobInfo()
            {
                Seq = new Guid("D608BE51-D170-4056-ADD4-A54EA20DC1C4"),
                Info = JsonUntityHelper.DeserializeStringToDictionary<string, object>(
                    "{\"_url\": \"https://tw.mall.yahoo.com/store/dcking\",   " +
                    $"\"_jobType\": \"PTT-PAGE\"," +
                    $"\"_pageStart\": \"{start}\"," +
                    $"\"_pageEnd\": \"{end}\"," +
                    $"\"_board\": \"{board}\"" +
                    "}")
            });
            return lj.AsEnumerable();
            //#else
            //return

            //  from x in CrawlFactory.CrawlDataJobListRepository.GetCrawlDataJobListDtos(resourceCount)
            //  select new JobInfo()
            //  {
            //      Info = JsonUntityHelper.DeserializeStringToDictionary<string, object>(x.job_info),
            //      Seq = x.seq
            //  };

            //#endif
        }
        private bool DoJob(JobInfo jobInfo)
        {
            var success = false;
            try
            {
                new JobWorkerFactory().GetJobWorker(jobInfo).DoJobFlow(CrawlFactory);
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
