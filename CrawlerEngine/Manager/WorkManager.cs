using CrawlerEngine.Common.Helper;
using CrawlerEngine.Driver;
using CrawlerEngine.JobWorker;
using CrawlerEngine.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrawlerEngine.Manager
{
    class WorkManager
    {
        private Condition resourseSetting;
        private List<string> mailTo;
        public Repository.Factory.CrawlFactory CrawlFactory;
        public void Process(int resourceCount, int browserCount, string machineName)
        {
            CrawlFactory = new Repository.Factory.CrawlFactory("POSTGRESSQL");
            //CrawlFactory = new Repository.Factory.CrawlFactory("MSSQL");
            WebDriverPool.InitDriver(browserCount);
            string vmIp = GetIp();
            List<Task> lt = new List<Task>();
            while (1 == 1)
            {

                //   freeDriverCount = WebDriverPool.GetFreeDriverConut();
                try
                {
                    foreach (var jobInfo in GetJobInfo(resourceCount, machineName))
                    {
                        jobInfo.PutToDic("machineName", machineName);
                        jobInfo.PutToDic("vmIp", vmIp);
                        lt.Add(Task.Run(() => DoJob(jobInfo)));
                    }

                    Task.WaitAll(lt.ToArray());
                    //Parallel.ForEach(GetJobInfo(freeDriverCount), jobInfo =>
                    //{
                    //    Task.Run(()=>DoJob(jobInfo));

                    //});
                }
                catch (Exception ex)
                {
                    // SendErrorEmail();
                    LoggerHelper._.Error(ex);
                }
                //Thread.Sleep(10000);
            }
        }

        #region 工作區
        private IEnumerable<JobInfo> GetJobInfo(int resourceCount, string machineName)
        {
            //#if (DEBUG)
            //List<JobInfo> lj = new List<JobInfo>();
            //lj.Add(new JobInfo()
            //{
            //    Seq = new Guid("D608BE51-D170-4056-ADD4-A54EA20DC1C4"),
            //    Info = JsonUntityHelper.DeserializeStringToDictionary<string, object>(
            //        "{\"_url\": \"https://tw.buy.yahoo.com/gdsale/%E7%A6%8F%E5%88%A9%E5%93%81-Apple-iPhone-8-64G-4-8772260.html?co_servername=FeebeeAPP01_web_\", " +
            //        "  \"_jobType\": \"YAHOOBUY-PRODUCT\"}")
            //});
            //return lj.AsEnumerable();
            //#else
            return

              from x in CrawlFactory.CrawlDataJobListRepository.GetCrawlDataJobListDtos(resourceCount, machineName)
              select new JobInfo()
              {
                  Info = JsonUntityHelper.DeserializeStringToDictionary<string, object>(x.job_info),
                  Seq = x.seq
              };

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

        private string GetIp()
        {
            string ip = string.Empty;
            var httpClient = new HttpClient();
            var httpResponse = httpClient.GetAsync("http://ip-api.com/json/").GetAwaiter().GetResult();
            string responseData = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            try
            {
                ip = JObject.Parse(responseData).Value<string>("query");
            }
            catch { }
            return ip;
        }


        #endregion
    }
}
