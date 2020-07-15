using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;
using System.Linq;

namespace CrawlerEngine.JobWorker.WorkClass
{
    /// <summary>
    /// 商品細節頁
    /// </summary>
    public class StockAllJobWorker : JobWorkerBase
    {
        public StockAllJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            crawler = new HttpCrawler(jobInfo);
        }
        public override JobInfo jobInfo { get; set; }
        public override ICrawler crawler { get; set; }

        protected override bool Crawl()
        {
            var success = false;
            try
            {
                // new HttpCrawler(new JobInfo() { Url = "https://24h.pchome.com.tw/store/DSAACI" }).DoCrawlerFlow();
                responseData = crawler.DoCrawlerFlow();
                success = true;
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
            }
            return success;
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

            Console.WriteLine(responseData);
            var t = responseData.Split('\n');

            var t2 = t.Select(x =>  x.Split(',') );
            //var htmlDoc = new HtmlDocument();
            //htmlDoc.LoadHtml(responseData);

            //crawlDataDetailOptions.price = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"PriceTotal\"]").InnerText;
            //crawlDataDetailOptions.name = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"NickContainer\"]").InnerText;
            //crawlDataDetailOptions.category = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"CONTENT\"]/div[1]/div[1]/div[2]").InnerText;
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
