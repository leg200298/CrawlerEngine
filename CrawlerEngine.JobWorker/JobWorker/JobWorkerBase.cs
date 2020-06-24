using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.JobWorker.Interface;
using CrawlerEngine.Models;
using System;

namespace CrawlerEngine.JobWorker
{
    abstract class JobWorkerBase : IJobWorker
    {
        public abstract JobInfo jobInfo { get; set; }
        public abstract ICrawler crawler { get; set; }
        public string responseData;
        /// <summary>
        /// 執行工作流程 ()
        /// </summary>
        public void DoJobFlow()
        {
            (bool, string) temp = (false, "");
            do
            {
                GotoNextPage(temp.Item2);
                CallCrawler();
                Validation();
                Parser();
                SaveData();
                temp = HasNextPage();
                SleepForAWhile(GetSleepTimeByJobInfo());
            } while (temp.Item1);

        }

        protected abstract bool CallCrawler();
        protected abstract bool Validation();
        protected abstract bool Parser();
        protected abstract bool SaveData();
        protected abstract (bool, string) HasNextPage();
        protected abstract bool GotoNextPage(string url);
        protected abstract int GetSleepTimeByJobInfo();
        protected abstract void SleepForAWhile(int sleepTime);
    }
}
