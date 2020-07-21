
using CrawlerEngine.Common.Extansion;
using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using static CrawlerEngine.Common.Enums.ElectronicBusiness;

namespace CrawlerEngine.JobWorker.WorkClass
{
    /// <summary>
    /// 商品細節頁
    /// </summary>
    public class YahooMallStoreJobWorker : JobWorkerBase
    {
        public YahooMallStoreJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;

        }
        private List<JobInfo> jobInfos = new List<JobInfo>();
        private HtmlDocument htmlDoc = new HtmlDocument();
        public override JobInfo jobInfo { get; set; }

        protected override bool Crawl()
        {
            var success = false;
            try
            {
                responseData = new WebCrawler(jobInfo).DoCrawlerFlow();
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

        protected override (bool, string) HasNextPage()
        {


            htmlDoc.LoadHtml(responseData);
            //   node = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"ypsaupg\"]/div[2]/a");
            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"ypsaupg\"]/div/a");

            var node = nodes.Where(x => x.InnerText.Contains("下一頁")).FirstOrDefault();
            if (node != null)
            {
                var url = new Uri(new Uri(jobInfo.Url), node.Attributes["href"].Value.Replace("&amp;", "&")).ToString();

                return (true, url);
            }
            else
            {
                return (false, "");
            }

        }
        protected override bool Parse()
        {

            htmlDoc.LoadHtml(responseData);
            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"ypsausi\"]/div[2]/ul/li/div[1]/table/tbody/tr/td/a");
            if (nodes is null) { return false; }
            foreach (var data in nodes)
            {
                var url = data.Attributes["href"].Value;
                if (url.StartsWith("https://tw.mall.yahoo.com/item"))
                {
                    jobInfos.Add(new JobInfo() { JobType = Platform.YahooMallProduct.GetDescription(), Url = $"{url}" });
                }
            }
            return true;
        }

        protected override bool SaveData()
        {
            foreach (var d in jobInfos)
            {
                Repository.Factory.CrawlFactory.CrawlDataJobListRepository.InsertOne(d, Platform.YahooMallProduct.GetDescription());
            }
            jobInfos.Clear();
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
