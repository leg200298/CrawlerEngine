using CrawlerEngine.Crawler;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

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
            JObject jObject = new JObject();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            CrawlDataDetailDto crawlDataDetailDto = new CrawlDataDetailDto();
            crawlDataDetailDto.Seq = jobInfo.Seq;
            jObject.Add("price", productPrice);
            jObject.Add("name", productName);
            jObject.Add("category", productCategory);
            crawlDataDetailDto.DetailData = jObject.ToString();
            crawlDataDetailDto.JobStatus = "end";
            crawlDataDetailDto.EndTime = DateTime.Now;
            Repository.Factory.CrawlFactory.CrawlDataDetailRepository.InsertDataDetail(crawlDataDetailDto);
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
