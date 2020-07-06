using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CrawlerEngine.JobWorker.WorkClass.Pchome
{

    /// <summary>
    /// 館分類頁
    /// </summary>
    class StoreJobWorker : JobWorkerBase
    {
        private List<JobInfo> jobInfos = new List<JobInfo>();
        private decimal sleepTime=0;
        private HtmlDocument htmlDoc = new HtmlDocument();
        public StoreJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            this.crawler = new WebCrawler(jobInfo);
        }
        public override JobInfo jobInfo { get; set; }
        public override ICrawler crawler { get; set; }

        protected override void UpdateJobStatusStart()
        {
            Repository.Factory.CrawlFactory.CrawlDataJobListRepository.UpdateStatusStart(jobInfo);
        }


        protected override bool GotoNextPage(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            else
            {
                if (url.StartsWith("//24h.pchome.com.tw"))
                {
                    jobInfo.Url = $"https:{url}";
                }

                return true;
            }
        }

        protected override bool Crawl()
        {
            var success = false;
            try
            {
                responseData = crawler.DoCrawlerFlow();
                success = true;
            }
            catch (Exception e)
            {
            }
            return success;
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

        protected override bool Parse()
        {

            htmlDoc.LoadHtml(responseData);
            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"ProdGridContainer\"]/dd/div/h5/a");

            foreach (var data in nodes)
            {
                var url = data.Attributes["href"].Value;
                if (url.StartsWith("//24h.pchome.com.tw"))
                {
                    jobInfos.Add(new JobInfo() { JobType = "PCHOME-PRODUCT", Url = $"https:{url}" });
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

        protected override decimal GetSleepTimeByJobInfo()
        {
            try
            {
                sleepTime = jobInfo.DriverSleepTime ??
                    2 + new Random().Next(3, 100) / 50;
            }
            catch (Exception) { }

            return sleepTime;
        }
        protected override (bool, string) HasNextPage()
        {

            htmlDoc.LoadHtml(responseData);

            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"PaginationContainer\"]/ul/li/a");

            foreach (var node in nodes)
            {
                if (node.InnerText == "下一頁")
                {
                    return (true, node.Attributes["href"].Value);
                }
            }
            return (false, "");
        }

        protected override void SleepForAWhile(decimal sleepTime)
        {
            Thread.Sleep((int)(sleepTime * 1000));
        }

        protected override void UpdateJobStatusEnd()
        {
            Repository.Factory.CrawlFactory.CrawlDataJobListRepository.UpdateStatusEnd(jobInfo);

        }


    }
}
