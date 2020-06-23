using CrawerEngine.Crawler.Interface;
using CrawerEngine.JobWorker.Interface;
using CrawerEngine.Models;
using System;

namespace CrawerEngine.JobWorker
{
    abstract class JobWorkerBase : IJobWorker
    {
        public JobInfo jobInfo;
        public ICrawler crawler;
        public string responseData;
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
