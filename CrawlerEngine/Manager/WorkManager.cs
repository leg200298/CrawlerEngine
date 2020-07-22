using CrawlerEngine.Common.Helper;
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
        private Queue<JobInfo> jobQueue = new Queue<JobInfo>();

        public void Process(int resourceCount)
        {
            ThreadPool.SetMaxThreads(5, 5);
            // WebDriverPool.InitDriver(resourceCount);
            var freeDriverCount = resourceCount;

            //while (1 == 1)
            {

                //   freeDriverCount = WebDriverPool.GetFreeDriverConut();
                try
                {
                    Console.WriteLine("Job Start");
                    List<Task> lt = new List<Task>();
                    GetJobInfo(freeDriverCount).ToList().ForEach((JobInfo) => jobQueue.Enqueue(JobInfo));

                    Console.WriteLine($"Job {jobQueue.Count()}");
                    //for (int i = 0; i <= all.Count(); i = i + 5)
                    //{
                    //    try { lt.Add(Task.Run(() => DoJob(all[i]))); } catch { }
                    //    try { lt.Add(Task.Run(() => DoJob(all[i + 1]))); } catch { }
                    //    try { lt.Add(Task.Run(() => DoJob(all[i + 2]))); } catch { }
                    //    try { lt.Add(Task.Run(() => DoJob(all[i + 3]))); } catch { }
                    //    try { lt.Add(Task.Run(() => DoJob(all[i + 4]))); } catch { }

                    //    foreach (var t in lt)
                    //    {
                    //        Console.WriteLine(t.Status + t.ToString());
                    //    }
                    //    Task.WaitAll(lt.ToArray());
                    //    lt.Clear();

                    //    //DoJob(jobInfo);
                    //}
                    do
                    {

                        DoJob(jobQueue.Dequeue());
                    } while (jobQueue.Count > 0);

                    //foreach (var jobInfo in all)
                    //{

                    //    Console.WriteLine(jobInfo.Seq + "Start");
                    //    DoJob(jobInfo);
                    //    //Thread.Sleep(10);
                    //    //ThreadPool.QueueUserWorkItem(new WaitCallback(DoJob),
                    //    //   jobInfo);

                    //}
                    //Parallel.ForEach(
                    //    GetJobInfo(freeDriverCount)
                    //    , jobInfo =>
                    //{


                    //    ThreadPool.QueueUserWorkItem(new WaitCallback(DoJob),
                    //       jobInfo);
                    //});
                    Console.WriteLine("Job End");
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Job Error {ex.Message}");
                    // SendErrorEmail();
                    LoggerHelper._.Error(ex);
                }
            }
        }


        #region 工作區
        private IEnumerable<JobInfo> GetJobInfo(int resourceCount)
        {
#if (DEBUG)

            List<JobInfo> lj = new List<JobInfo>();
            lj.Add(new JobInfo()
            {
                Seq = new Guid("063AA19C-FB8D-448C-B144-0AEA877A8F92"),
                Info = JsonUntityHelper.DeserializeStringToDictionary<string, object>(
                    "{\"_jobType\": \"STOCK-GPEV\",\"_url\": \"https://cronjob.uanalyze.com.tw/fetch/GrowthPEValuation/1409\"}"),

                JobCycle = "Monthly",
                JobRegisterTime = new DateTime(2020, 5, 17, 7, 14, 7, 633)
            });
            //return

            // from x in Repository.Factory.CrawlFactory.StockJobListRepository.GetStockJobListDtos()
            // select new JobInfo()
            // {
            //     JobType = x.JobType,
            //     Info = JsonUntityHelper.DeserializeStringToDictionary<string, object>(x.JobInfo),
            //     Seq = x.Seq,
            //     JobCycle = x.JobCycle,
            //     JobRegisterTime = x.RegisterTime
            // };


            //lj.Add(new JobInfo()
            //{
            //    Seq = new Guid("DDCD3448-090F-4E69-82E5-6F4B1C444C29"),
            //    Info = JsonUntityHelper.DeserializeStringToDictionary<string, object>(
            //      "{\"_jobType\": \"STOCK-GPEV\",\"_url\": \"https://cronjob.uanalyze.com.tw/fetch/GrowthPEValuation/00688L\"}"),

            //    JobCycle = "Monthly",
            //    JobRegisterTime = new DateTime(2020, 7, 17, 7, 14, 7, 633)
            //});
            //lj.Add(new JobInfo()
            //{
            //    Seq = new Guid("8C92FA61-89C4-4984-8C63-6A55A1F2C0F1"),
            //    Info = JsonUntityHelper.DeserializeStringToDictionary<string, object>(
            //      "{\"_jobType\": \"STOCK-CEE\",\"_url\": \"https://cronjob.uanalyze.com.tw/fetch/Consensus_EPS_Estimatetable/1762\"}"),

            //    JobCycle = "Quarter",
            //    JobRegisterTime = new DateTime(2020, 7, 17, 7, 14, 7, 633)
            //});
            return lj.AsEnumerable();
#else
            return

              from x in Repository.Factory.CrawlFactory.StockJobListRepository.GetStockJobListDtos()
              select new JobInfo()
              {
                  JobType = x.JobType,
                  Info = JsonUntityHelper.DeserializeStringToDictionary<string, object>(x.JobInfo),
                  Seq = x.Seq,
                  JobCycle = x.JobCycle,
                  JobRegisterTime = x.RegisterTime
              };

#endif
        }
        public void DoJob(object jobInfo)
        {
            var s = jobInfo as JobInfo;
            var success = false;
            try
            {
                new JobWorkerFactory().GetJobWorker(s).DoJobFlow();
                success = true;
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
            }

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
