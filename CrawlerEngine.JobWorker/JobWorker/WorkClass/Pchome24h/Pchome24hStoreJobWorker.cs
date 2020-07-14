using CrawlerEngine.Common.Extansion;
using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading;
using static CrawlerEngine.Common.Enums.ElectronicBusiness;

namespace CrawlerEngine.JobWorker.WorkClass
{

    /// <summary>
    /// 館分類頁
    /// </summary>
    class Pchome24hStoreJobWorker : JobWorkerBase
    {
        private List<JobInfo> jobInfos = new List<JobInfo>();
        private HtmlDocument htmlDoc = new HtmlDocument();
        public Pchome24hStoreJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            crawler = new WebCrawler(jobInfo);
        }
        public override JobInfo jobInfo { get; set; }
        public override ICrawler crawler { get; set; }



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
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
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
            responseData = responseData.Replace("<div class=\"prod_info\">", "");
            htmlDoc.LoadHtml(responseData);
            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"ProdGridContainer\"]/dd/h5/a");
            if (nodes is null) { return false; }
            foreach (var data in nodes)
            {
                var url = data.Attributes["href"].Value;
                if (url.StartsWith("//24h.pchome.com.tw"))
                {
                    jobInfos.Add(new JobInfo() { JobType = Platform.Pchome24hProduct.GetDescription(), Url = $"https:{url}" });
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
            jobInfos.Clear();
            return true;

        }

        protected override (bool, string) HasNextPage()
        {

            htmlDoc.LoadHtml(responseData);

            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"PaginationContainer\"]/ul/li/a");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    if (node.InnerText == "下一頁")
                    {
                        return (true, node.Attributes["href"].Value);
                    }
                }
            }
            return (false, "");
        }

        protected override void SleepForAWhile(decimal sleepTime)
        {
            Thread.Sleep((int)(sleepTime * 1000));
        }



    }
}
