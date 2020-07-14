using CrawlerEngine.Common.Extansion;
using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using static CrawlerEngine.Common.Enums.ElectronicBusiness;

namespace CrawlerEngine.JobWorker.WorkClass
{
    /// <summary>
    /// 商品細節頁
    /// </summary>
    public class YahooMallSearchJobWorker : JobWorkerBase
    {
        public YahooMallSearchJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            crawler = new WebCrawler(jobInfo);
        }
        private List<JobInfo> jobInfos = new List<JobInfo>();
        private HtmlDocument htmlDoc = new HtmlDocument();
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
            var node = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"isoredux-root\"]/div/div/div/div/div[1]/span");
            var q = node.Attributes["innerText"].Value;
            var total = Convert.ToInt32(Regex.Match(q, @"\d+").Value);
            var totalPage = total / 60 + 1;
            Uri myUri = new Uri(jobInfo.Url);
            string paramKW = HttpUtility.ParseQueryString(myUri.Query).Get("kw");
            string paramP = HttpUtility.ParseQueryString(myUri.Query).Get("p");
            string paramPage = HttpUtility.ParseQueryString(myUri.Query).Get("pg");
            if (totalPage < Convert.ToInt32(paramPage))
            {
                return (false, "");
            }
            else
            {

                var url = new Uri(myUri, $"?cid=0&clv=0&kw={paramKW}&p={paramP}&pg={Convert.ToInt32(paramPage) + 1}").ToString();

                return (true, url);
            }

        }

        protected override bool Parse()
        {
            //未完成
            responseData = responseData.Replace("<div class=\"prod_info\">", "");
            htmlDoc.LoadHtml(responseData);
            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"isoredux-root\"]/div/div/div/div/div/ul/li/a");
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
            //未完成
            foreach (var d in jobInfos)
            {
                Repository.Factory.CrawlFactory.CrawlDataJobListRepository.InsertOne(d);
            }
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
