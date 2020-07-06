using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.JobWorker.Interface;
using CrawlerEngine.Models;
using CrawlerEngine.Models.Models;

namespace CrawlerEngine.JobWorker
{
    abstract class JobWorkerBase : IJobWorker
    {
        public abstract JobInfo jobInfo { get; set; }
        public abstract ICrawler crawler { get; set; }
        public string responseData;
        protected JsonOptions crawlDataDetailOptions = new JsonOptions();
        /// <summary>
        /// 執行工作流程 ()
        /// </summary>
        public void DoJobFlow()
        {
            UpdateJobStatusStart();
            (bool, string) temp = (false, "");
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

        protected abstract void UpdateJobStatusEnd();
        protected abstract void UpdateJobStatusStart();

        protected abstract bool Crawl();
        protected abstract bool Validate();
        protected abstract bool Parse();
        protected abstract bool SaveData();
        protected abstract (bool, string) HasNextPage();
        protected abstract bool GotoNextPage(string url);
        protected abstract decimal GetSleepTimeByJobInfo();
        protected abstract void SleepForAWhile(decimal sleepTime);
    }
}
