using CrawlerEngine.Crawler;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;

namespace CrawlerEngine.JobWorker.WorkClass
{
    class PchomeProductJobWorker : JobWorkerBase
    {
        private string productPrice = string.Empty;
        private string productName = string.Empty;
        private string productCategory = string.Empty;
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
                responseData = crawler.DoCrawlerFlow();
                success = true;
            }
            catch (Exception e)
            {
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

            return (false, "");
        }

        protected override bool Parse()
        {

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseData);
            productPrice = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"PriceTotal\"]").InnerText;
            productName = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"NickContainer\"]").InnerText;
            productCategory = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"CONTENT\"]/div[1]/div[1]/div[2]").InnerText;
            return false;
        }

        protected override bool SaveData()
        {
            Console.WriteLine($"productPrice: {productPrice}");
            Console.WriteLine($"productName: {productName}");
            Console.WriteLine($"productCategory: {productCategory}");
            return false;

        }

        protected override void SleepForAWhile(int sleepTime)
        {

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
