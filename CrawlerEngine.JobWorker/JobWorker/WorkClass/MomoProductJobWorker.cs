﻿using CrawlerEngine.Crawler;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Models;
using System;

namespace CrawlerEngine.JobWorker.WorkClass
{
    class MomoProductJobWorker : JobWorkerBase

    {
        public MomoProductJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            this.crawler = new CrawlerFactory().GetCrawler(jobInfo);
        }
        public override JobInfo jobInfo { get; set; }
        public override ICrawler crawler { get; set; }

        protected override bool Crawl()
        {
            throw new NotImplementedException();
        }

        protected override decimal GetSleepTimeByJobInfo()
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

        protected override bool Parse()
        {
            throw new NotImplementedException();
        }

        protected override bool SaveData()
        {
            throw new NotImplementedException();
        }

        protected override void SleepForAWhile(decimal sleepTime)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateJobStatusEnd()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateJobStatusStart()
        {
            throw new NotImplementedException();
        }

        protected override bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}
