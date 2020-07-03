using CrawlerEngine.Crawler;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;

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
            crawlDataDetailOptions.price = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"PriceTotal\"]").InnerText;
            crawlDataDetailOptions.name = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"NickContainer\"]").InnerText;
            crawlDataDetailOptions.category = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"CONTENT\"]/div[1]/div[1]/div[2]").InnerText;
            return false;
        }

        protected override bool SaveData()
        {
            CrawlDataDetailDto crawlDataDetailDto = new CrawlDataDetailDto()
            {
                Seq = jobInfo.Seq,
                JobStatus = "end",
                EndTime = DateTime.Now,
                DetailData = crawlDataDetailOptions.GetJsonString()
            };

            Repository.Factory.CrawlFactory.CrawlDataDetailRepository.InsertDataDetail(crawlDataDetailDto);
            return false;

        }

        protected override void SleepForAWhile(int sleepTime)
        {

        }

        protected override void UpdateJobStatusEnd()
        {
            CrawlDataJobListDto crawlDataJobListDto = new CrawlDataJobListDto()
            {
                Seq = jobInfo.Seq
            };
            Repository.Factory.CrawlFactory.CrawlDataJobListRepository.UpdateStatusEnd(crawlDataJobListDto);

        }

        protected override void UpdateJobStatusStart()
        {
            CrawlDataJobListDto crawlDataJobListDto = new CrawlDataJobListDto()
            {
                Seq = jobInfo.Seq
            };
            Repository.Factory.CrawlFactory.CrawlDataJobListRepository.UpdateStatusStart(crawlDataJobListDto);
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
