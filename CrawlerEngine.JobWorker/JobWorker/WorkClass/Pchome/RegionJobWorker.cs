using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace CrawlerEngine.JobWorker.WorkClass.Pchome
{
    /// <summary>
    /// 中分類頁
    /// </summary>
    class RegionJobWorker : JobWorkerBase
    {
        public RegionJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            crawler = new WebCrawler(jobInfo);
        }
        private List<JobInfo> jobInfos = new List<JobInfo>();
        public override JobInfo jobInfo { get; set; }
        public override ICrawler crawler { get; set; }

        protected override bool Crawl()
        {
            var success = false;
            try
            {
                responseData = crawler.DoCrawlerFlow();
                success = true;
            }
            catch (Exception)
            {
            }
            return success;
        }

        protected override decimal GetSleepTimeByJobInfo()
        {
            return 1000;
        }

        protected override bool GotoNextPage(string url)
        {
            return false;
        }

        protected override (bool, string) HasNextPage()
        {

            return (false, "");
        }

        protected override bool Parse()
        {

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseData);
            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"Block12Container50\"]/dd/div/h5/a");

            foreach (var data in nodes)
            {
                var url = data.Attributes["href"].Value;
                if (url.StartsWith("/prod/"))
                {
                    jobInfos.Add(new JobInfo() { JobType = "PCHOME-PRODUCT", Url = $"https://24h.pchome.com.tw{url}" });
                }
            }
            return true;
        }

        protected override bool SaveData()
        {
            foreach (var d in jobInfos)
            {
                Repository.Factory.CrawlFactory.CrawlDataJobListRepository.InsertOne(d);
            }
            return true;

        }

        protected override void SleepForAWhile(decimal sleepTime)
        {

        }

        protected override void UpdateJobStatusEnd()
        {
            Repository.Factory.CrawlFactory.CrawlDataJobListRepository.UpdateStatusEnd(jobInfo);

        }

        protected override void UpdateJobStatusStart()
        {
            Repository.Factory.CrawlFactory.CrawlDataJobListRepository.UpdateStatusStart(jobInfo);
        }

        protected override bool Validate()
        {
            if (string.IsNullOrEmpty(responseData))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
