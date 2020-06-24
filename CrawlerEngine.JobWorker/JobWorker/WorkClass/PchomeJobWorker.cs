using CrawlerEngine.Crawler;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.JobWorker;
using CrawlerEngine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.JobWorker.WorkClass
{
    class PchomeJobWorker : JobWorkerBase
    {
        public PchomeJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            this.crawler = new CrawlerFactory().GetCrawler(jobInfo);
        }
        public override JobInfo jobInfo { get; set; }
        public override ICrawler crawler { get; set; }

        protected override bool CallCrawler()
        {
            var success = false;
            try
            {
                crawler.DoCrawlerFlow();
                success = true;
            }
            catch (Exception e) { 
            }
            return success;
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
