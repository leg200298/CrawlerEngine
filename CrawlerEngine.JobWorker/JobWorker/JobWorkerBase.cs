using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.JobWorker.Interface;
using CrawlerEngine.Models;
using CrawlerEngine.Models.Models;
using System;
using System.Collections.Generic;

namespace CrawlerEngine.JobWorker
{
    public abstract class JobWorkerBase : IJobWorker
    {
        public abstract JobInfo jobInfo { get; set; }
        public abstract ICrawler crawler { get; set; }
        public string responseData;

        private decimal sleepTime = 0;
        protected JsonOptions crawlDataDetailOptions = new JsonOptions();
        protected List<JsonOptions> lCrawlDataDetailOptions = new List<JsonOptions>();
        /// <summary>
        /// 執行工作流程 ()
        /// </summary>
        public void DoJobFlow()
        {
            if (!DesideByCycle()) { return; }
            UpdateJobStatusStart();
            (bool, string) temp = (false, "");
            try
            {
                LoggerHelper._.Info($"{jobInfo.Seq}  Start");
                do
                {
                    GotoNextPage(temp.Item2);
                    Crawl();

                    LoggerHelper._.Info($"{jobInfo.Seq}  GetData");
                    if (Validate())
                    {
                        LoggerHelper._.Info($"{jobInfo.Seq}  Validate");
                        if (Parse())
                        {
                            LoggerHelper._.Info($"{jobInfo.Seq}  Parse");
                            SaveData();
                            LoggerHelper._.Info($"{jobInfo.Seq}  SaveData");
                        }
                        temp = HasNextPage();
                        SleepForAWhile(GetSleepTimeByJobInfo());
                    }
                } while (temp.Item1);
                UpdateJobStatusEnd();
                LoggerHelper._.Info($"{jobInfo.Seq}  End");
            }
            catch (Exception e)
            {
                LoggerHelper._.Error(e, jobInfo.Seq.ToString() + responseData);
                jobInfo.ErrorInfo = e.Message;
                UpdateJobStatusFail();

            }

        }

        private bool DesideByCycle()
        {
            var cycle = jobInfo.JobCycle;
            var regDate = jobInfo.JobRegisterTime;
            var nowDate = DateTime.UtcNow;
            List<int> target = new List<int>() { 4, 7, 9, 12 };
            if (cycle == "Daily") return true;

            if (cycle == "Monthly" && (regDate.AddMonths(1) < nowDate)) return true;

            if (cycle == "Quarter" && target.Contains(nowDate.Month) && regDate.Month != nowDate.Month) return true;

            return false;

        }

        private void UpdateJobStatusStart()
        {
            Repository.Factory.CrawlFactory.StockJobListRepository.UpdateStatusStart(jobInfo);

        }

        protected abstract bool Crawl();
        protected abstract bool Validate();
        protected abstract bool Parse();
        protected abstract bool SaveData();
        protected abstract (bool, string) HasNextPage();
        protected abstract bool GotoNextPage(string url);
        private decimal GetSleepTimeByJobInfo()
        {
            try
            {
                sleepTime = jobInfo.DriverSleepTime ??
                    2 + new Random().Next(3, 100) / 50;
            }
            catch (Exception ex)
            {

                LoggerHelper._.Error(ex);
            }

            return sleepTime;
        }
        protected abstract void SleepForAWhile(decimal sleepTime);
        private void UpdateJobStatusEnd()
        {
            Repository.Factory.CrawlFactory.StockJobListRepository.UpdateStatusEnd(jobInfo);
        }
        private void UpdateJobStatusFail()
        {
            Repository.Factory.CrawlFactory.StockJobListRepository.UpdateJobStatusFail(jobInfo);
        }

    }
}
