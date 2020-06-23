using CrawerEngine.JobWorker.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawerEngine.JobWorker
{
    abstract class JobWorkerBase : IJobWorker
    {
        public void DoJobFlow()
        {
            throw new NotImplementedException();
        }

        protected abstract bool CallCrawler();
        protected abstract bool Validation();
        protected abstract bool Parser();
        protected abstract bool SaveData();
        protected abstract (bool, string) HasNextPage();
        protected abstract bool GotoNextPage();
        protected abstract int GetSleepTimeByJobInfo();
        protected abstract void SleepForAWhile();
    }
}
