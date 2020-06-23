using CrawerEngine.JobWorker;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.JobWorker.WorkClass
{
    class PchomeJobWorker : JobWorkerBase
    {
        protected override bool CallCrawler()
        {
            throw new NotImplementedException();
        }

        protected override int GetSleepTimeByJobInfo()
        {
            throw new NotImplementedException();
        }

        protected override bool GotoNextPage(string url)
        {
            throw new NotImplementedException();
        }

        protected override (bool, string) HasNextPage()
        {
            throw new NotImplementedException();
        }

        protected override bool Parser()
        {
            throw new NotImplementedException();
        }

        protected override bool SaveData()
        {
            throw new NotImplementedException();
        }

        protected override void SleepForAWhile(int sleepTime)
        {
            throw new NotImplementedException();
        }

        protected override bool Validation()
        {
            throw new NotImplementedException();
        }
    }
}
