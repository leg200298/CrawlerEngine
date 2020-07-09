using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.JobWorker.Interface;
using CrawlerEngine.Models;
using CrawlerEngine.Models.Models;
using System;

namespace CrawlerEngine.JobWorker
{
    public abstract class JobWorkerBase : IJobWorker
    {
        public abstract JobInfo jobInfo { get; set; }
        public abstract ICrawler crawler { get; set; }
        public string responseData;

        private decimal sleepTime = 0;
        protected JsonOptions crawlDataDetailOptions = new JsonOptions();
        /// <summary>
        /// 執行工作流程 ()
        /// </summary>
        public void DoJobFlow()
        {
            UpdateJobStatusStart();
            (bool, string) temp = (false, "");
            try
            {
                do
                {
                    GotoNextPage(temp.Item2);
                    Crawl();
                    if (Validate())
                    {
                        if (Parse())
                        {
                            SaveData();
                        }
                        temp = HasNextPage();
                        SleepForAWhile(GetSleepTimeByJobInfo());
                    }
                } while (temp.Item1);
                UpdateJobStatusEnd();
            }
            catch (Exception e)
            {
                LoggerHelper._.Error(e,jobInfo.Seq.ToString()+responseData);
                UpdateJobStatusFail();

            }

        }
        private void UpdateJobStatusStart()
        {
            //  Repository.Factory.CrawlFactory.CrawlDataJobListRepository.UpdateStatusStart(jobInfo);

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
            Repository.Factory.CrawlFactory.CrawlDataJobListRepository.UpdateStatusEnd(jobInfo);
        }
        private void UpdateJobStatusFail()
        {
            Repository.Factory.CrawlFactory.CrawlDataJobListRepository.UpdateJobStatusFail(jobInfo);
        }

    }
}
