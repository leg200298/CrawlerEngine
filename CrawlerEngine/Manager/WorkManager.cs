using CrawlerEngine.Common.Helper;
using CrawlerEngine.JobWorker;
using CrawlerEngine.Models;
using System;
using System.Collections.Concurrent;
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
            // WebDriverPool.InitDriver(resourceCount);
            var freeDriverCount = 100;
            int i = 1;
            do
            {


                //     freeDriverCount = WebDriverPool.GetFreeDriverConut();
                try
                {
                    Console.WriteLine($"{i.ToString() } Start");
                    Parallel.ForEach(GetJobInfo(i, freeDriverCount), jobInfo =>
                     {
                         DoJob(jobInfo);
                     });

                    //    DoJob(new JobInfo()
                    //    {

                    //        Seq = new Guid("D608BE51-D170-4056-ADD4-A54EA20DC1C4"),
                    //        Info = JsonUntityHelper.DeserializeStringToDictionary<string, object>(
                    //"{\"_url\": \"https://redmine.etzone.net/issues/" + i.ToString() + "\"," +
                    //"   \"_jobType\": \"RedMine\"}")
                    //    });

                    Console.WriteLine($"{i.ToString() } End");
                }
                catch (Exception ex)
                {
                    LoggerHelper._.Error(ex);
                }
                i = i + freeDriverCount;
            } while (i < 30000);
        }

        private OrderablePartitioner<object> GetList(int i)
        {
            throw new NotImplementedException();
        }


        #region 工作區
        private IEnumerable<JobInfo> GetJobInfo(int i, int resourceCount)
        {
#if (DEBUG)
            List<JobInfo> lj = new List<JobInfo>();
            for (int j = i; j < i + resourceCount; ++j)
            {
                lj.Add(new JobInfo()
                {
                    Seq = new Guid("D608BE51-D170-4056-ADD4-A54EA20DC1C4"),
                    Info = JsonUntityHelper.DeserializeStringToDictionary<string, object>(
                        "{\"_url\": \"https://redmine.etzone.net/issues/" + j.ToString() + "\", " +
                        "  \"_jobType\": \"RedMine\"}")
                });
            }
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
