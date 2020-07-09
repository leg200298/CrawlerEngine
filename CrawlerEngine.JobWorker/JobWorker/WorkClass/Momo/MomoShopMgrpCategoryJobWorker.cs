using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CrawlerEngine.JobWorker.WorkClass
{
    public class MomoShopMgrpCategoryJobWorker : JobWorkerBase
    {
        public override JobInfo jobInfo { get; set; }
        public override ICrawler crawler { get; set; }

        private List<JobInfo> jobInfos = new List<JobInfo>();
        private HtmlDocument htmlDoc = new HtmlDocument();

        public MomoShopMgrpCategoryJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            crawler = new WebCrawler(jobInfo);
        }

        protected override bool GotoNextPage(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            else
            {
                jobInfo.Url = url;
                return true;
            }
        }

        protected override bool Crawl()
        {
            try
            {
                responseData = crawler.DoCrawlerFlow();
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
                return false;
            }
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
            try
            {
                htmlDoc.LoadHtml(responseData);
                var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@class='newClassificationFilterArea']//a[contains(@href, 'cateGoods.momo')]");
                if (nodes is null) { return false; }
                foreach (var data in nodes)
                {
                    string href = HtmlEntity.DeEntitize(data.Attributes["href"].Value);
                    jobInfos.Add(new JobInfo()
                    {
                        Seq = Guid.NewGuid(),
                        JobType = "MOMOSHOP-DGRPCATEGORY",
                        Url = href.StartsWith("https://m.momoshop.com.tw") ? href : $"https://m.momoshop.com.tw{href}"
                    });
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
                return false;
            }
        }

        protected override bool SaveData()
        {
            try
            {
                foreach (var job in jobInfos)
                {
                    Repository.Factory.CrawlFactory.CrawlDataJobListRepository.InsertOne(job);
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
                return false;
            }
        }

        protected override (bool, string) HasNextPage()
        {
            return (false, "");
        }


        protected override void SleepForAWhile(decimal sleepTime)
        {
            Thread.Sleep((int)(sleepTime * 1000));
        }
    }
}
