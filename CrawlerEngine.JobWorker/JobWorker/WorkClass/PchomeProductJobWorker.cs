using CrawlerEngine.Crawler;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.JobWorker;
using CrawlerEngine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.JobWorker.WorkClass
{
    class PchomeProductJobWorker : JobWorkerBase
    {
        public PchomeProductJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            this.crawler = new CrawlerFactory().GetCrawler(jobInfo);
        }
        public override JobInfo jobInfo { get; set; }
        public override ICrawler crawler { get; set; }

        protected override bool Crawl()
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
            return 1000;
        }

        protected override bool GotoNextPage(string url)
        {
            return false;
        }

        protected override (bool, string) HasNextPage()
        {

            return (false,"");
        }

        protected override bool Parse()
        {

            return false;
        }

        protected override bool SaveData()
        {
            return false;

        }

        protected override void SleepForAWhile(int sleepTime)
        {
         
        }

        protected override bool Validate()
        {

            return false;
        }
    }
}
