using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;

namespace CrawlerEngine.JobWorker.WorkClass.Pchome
{
    /// <summary>
    /// 商品細節頁
    /// </summary>
    public class ProductJobWorker : JobWorkerBase
    {
        public ProductJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            crawler = new WebCrawler(jobInfo);
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
            crawlDataDetailOptions.price = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"PriceTotal\"]").InnerText;
            crawlDataDetailOptions.name = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"NickContainer\"]").InnerText;
            crawlDataDetailOptions.category = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"CONTENT\"]/div[1]/div[1]/div[2]").InnerText;
            return true;
        }

        protected override bool SaveData()
        {
            CrawlDataDetailDto crawlDataDetailDto = new CrawlDataDetailDto()
            {
                Seq = jobInfo.Seq,
                JobStatus = "end",
                EndTime = DateTime.UtcNow,
                DetailData = crawlDataDetailOptions.GetJsonString()
            };

            Repository.Factory.CrawlFactory.CrawlDataDetailRepository.InsertDataDetail(crawlDataDetailDto);
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
